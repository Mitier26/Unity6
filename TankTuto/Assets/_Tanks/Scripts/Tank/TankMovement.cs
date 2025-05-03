using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Tanks.Complete
{
    // TankShooting보다 먼저 실행되도록 설정합니다. TankShooting은 GameManager가 설정되지 않은 경우 이 컴포넌트에서 InputUser를 가져옵니다.
    // (학습용 테스트 장면에서 탱크를 단독으로 테스트할 때 사용됩니다)
    [DefaultExecutionOrder(-10)]
    public class TankMovement : MonoBehaviour
    {
        [Tooltip("플레이어 번호. 탱크 선택 메뉴가 없을 경우, Player 1은 왼쪽 키보드, Player 2는 오른쪽 키보드를 사용합니다")]
        public int m_PlayerNumber = 1; // 이 탱크가 어떤 플레이어에 속하는지 식별하는 데 사용됩니다. 탱크 매니저에서 설정됩니다.

        [Tooltip("탱크가 이동하는 속도 (유닛/초)")]
        public float m_Speed = 12f; // 앞으로 또는 뒤로 이동하는 속도입니다.

        [Tooltip("탱크가 회전하는 속도 (도/초)")]
        public float m_TurnSpeed = 180f; // 초당 회전하는 속도입니다.

        [Tooltip("true로 설정 시, 좌우 회전이 아닌 눌린 방향으로 직접 회전하며 이동합니다")]
        public bool m_IsDirectControl;

        public AudioSource m_MovementAudio; // 엔진 소리를 재생하는 오디오 소스입니다. 사격용 오디오 소스와는 다릅니다.
        public AudioClip m_EngineIdling; // 탱크가 정지 상태일 때 재생되는 오디오 클립입니다.
        public AudioClip m_EngineDriving; // 탱크가 이동 중일 때 재생되는 오디오 클립입니다.
        public float m_PitchRange = 0.2f; // 엔진 사운드의 피치가 바뀌는 범위입니다.

        [Tooltip("true로 설정 시, 이 탱크는 플레이어가 아니라 컴퓨터가 제어합니다")]
        public bool m_IsComputerControlled = false; // 이 탱크가 AI에 의해 제어되는지 여부입니다.

        [HideInInspector]
        public TankInputUser m_InputUser; // 이 탱크의 Input User 컴포넌트입니다. Input Actions를 포함합니다.

        public Rigidbody Rigidbody => m_Rigidbody; // 외부에서 Rigidbody에 접근할 수 있게 프로퍼티 제공

        public int ControlIndex { get; set; } = -1; // 컨트롤 인덱스를 정의합니다. 1 = 왼쪽 키보드/패드, 2 = 오른쪽 키보드, -1 = 제어 없음

        private string m_MovementAxisName; // 전진 및 후진 입력 축 이름
        private string m_TurnAxisName; // 회전 입력 축 이름
        private Rigidbody m_Rigidbody; // 탱크를 이동시키기 위해 사용하는 Rigidbody 참조
        private float m_MovementInputValue; // 현재 전진/후진 입력 값
        private float m_TurnInputValue; // 현재 회전 입력 값
        private float m_OriginalPitch; // 장면 시작 시 오디오 소스의 피치
        private ParticleSystem[] m_particleSystems; // 탱크에 사용되는 파티클 시스템들의 참조

        private InputAction m_MoveAction; // 이동에 사용되는 InputAction입니다. TankInputUser에서 가져옵니다.
        private InputAction m_TurnAction; // 회전에 사용되는 InputAction입니다. TankInputUser에서 가져옵니다.

        private Vector3 m_RequestedDirection; // Direct Control 모드일 때, 사용자가 원하는 방향을 저장합니다

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();

            // TankInputUser 컴포넌트가 없으면 추가합니다.
            m_InputUser = GetComponent<TankInputUser>();
            if (m_InputUser == null)
                m_InputUser = gameObject.AddComponent<TankInputUser>();
        }

        private void OnEnable()
        {
            // 탱크가 다시 활성화되었을 때, 물리 연산을 적용하기 위해 kinematic 비활성화
            m_Rigidbody.isKinematic = false;

            // 입력 초기화
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // 파티클 시스템을 재생하여 위치 변화 시 흔적이 남지 않도록 함
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }

        private void OnDisable()
        {
            // 물리 연산을 멈추기 위해 kinematic 활성화
            m_Rigidbody.isKinematic = true;

            // 파티클 시스템 중지 (스폰 이동 중 잔상 방지)
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }

        private void Start()
        {
            // AI 제어가 필요한 경우 TankAI 컴포넌트가 없으면 추가합니다.
            if (m_IsComputerControlled)
            {
                var ai = GetComponent<TankAI>();
                if (ai == null)
                {
                    gameObject.AddComponent<TankAI>();
                }
            }

            // ControlIndex가 설정되지 않았다면 PlayerNumber를 기반으로 지정합니다.
            if (ControlIndex == -1 && !m_IsComputerControlled)
            {
                ControlIndex = m_PlayerNumber;
            }

            var mobileControl = FindAnyObjectByType<MobileUIControl>();

            // 모바일 환경이거나 모바일 테스트 시, 가상 게임패드 장치를 연결합니다.
            if (mobileControl != null && ControlIndex == 1)
            {
                m_InputUser.SetNewInputUser(InputUser.PerformPairingWithDevice(mobileControl.Device));
                m_InputUser.ActivateScheme("Gamepad");
            }
            else
            {
                // 키보드 왼쪽/오른쪽 스킴을 설정합니다.
                m_InputUser.ActivateScheme(ControlIndex == 1 ? "KeyboardLeft" : "KeyboardRight");
            }

            m_MovementAxisName = "Vertical";
            m_TurnAxisName = "Horizontal";

            // TankInputUser로부터 액션을 찾고 활성화합니다.
            m_MoveAction = m_InputUser.ActionAsset.FindAction(m_MovementAxisName);
            m_TurnAction = m_InputUser.ActionAsset.FindAction(m_TurnAxisName);

            m_MoveAction.Enable();
            m_TurnAction.Enable();

            // 원래의 엔진 사운드 피치를 저장해둡니다.
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update()
        {
            // AI가 아닌 경우에만 사용자 입력을 받아옵니다.
            if (!m_IsComputerControlled)
            {
                m_MovementInputValue = m_MoveAction.ReadValue<float>();
                m_TurnInputValue = m_TurnAction.ReadValue<float>();
            }

            EngineAudio();
        }

        private void EngineAudio()
        {
            // 이동 입력이 거의 없을 때
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                // 주행 클립이 재생 중이라면 정지 상태로 변경합니다.
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // 정지 클립이 재생 중이라면 주행 사운드로 변경합니다.
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }

        private void FixedUpdate()
        {
            // Gamepad나 DirectControl 모드에서는 카메라 방향 기준으로 이동 방향을 설정합니다.
            if (m_InputUser.InputUser.controlScheme.Value.name == "Gamepad" || m_IsDirectControl)
            {
                var camForward = Camera.main.transform.forward;
                camForward.y = 0; // 수평 방향만 고려
                camForward.Normalize();
                var camRight = Vector3.Cross(Vector3.up, camForward); // 카메라 우측 벡터 계산

                m_RequestedDirection = (camForward * m_MovementInputValue + camRight * m_TurnInputValue);
            }

            Move();
            Turn();
        }

        private void Move()
        {
            float speedInput = 0.0f;

            // DirectControl 모드라면 회전 각도에 따라 속도를 줄입니다.
            if (m_InputUser.InputUser.controlScheme.Value.name == "Gamepad" || m_IsDirectControl)
            {
                speedInput = m_RequestedDirection.magnitude; // 방향 입력 세기
                // 방향과 현재 정면 사이의 각도에 따라 속도 보정 (90도 이내는 그대로, 이상은 감소)
                speedInput *= 1.0f - Mathf.Clamp01((Vector3.Angle(m_RequestedDirection, transform.forward) - 90) / 90.0f);
            }
            else
            {
                // 일반 탱크 방식: 위 방향 입력만큼 전진
                speedInput = m_MovementInputValue;
            }

            // 이동 벡터 계산 = 정면 * 입력 세기 * 속도 * 시간
            Vector3 movement = transform.forward * speedInput * m_Speed * Time.deltaTime;

            // Rigidbody 위치 이동
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }

        private void Turn()
        {
            Quaternion turnRotation;

            // DirectControl 혹은 Gamepad일 경우
            if (m_InputUser.InputUser.controlScheme.Value.name == "Gamepad" || m_IsDirectControl)
            {
                // 원하는 방향과 현재 방향 사이의 회전 각도를 계산합니다.
                float angleTowardTarget = Vector3.SignedAngle(m_RequestedDirection, transform.forward, transform.up);
                float rotatingAngle = Mathf.Sign(angleTowardTarget) * Mathf.Min(Mathf.Abs(angleTowardTarget), m_TurnSpeed * Time.deltaTime);
                turnRotation = Quaternion.AngleAxis(-rotatingAngle, Vector3.up); // 원하는 만큼만 회전
            }
            else
            {
                // 일반 탱크 방식: 좌우 입력값에 따라 회전 각도를 결정
                float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
                turnRotation = Quaternion.Euler(0f, turn, 0f); // Y축 회전만 적용
            }

            // Rigidbody 회전 적용
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }
}
