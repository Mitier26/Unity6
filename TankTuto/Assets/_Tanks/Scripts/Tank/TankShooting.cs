using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tanks.Complete
{
    public class TankShooting : MonoBehaviour
    {
        public Rigidbody m_Shell;                   // 포탄 프리팹입니다.
        public Transform m_FireTransform;           // 포탄이 생성되는 탱크의 자식 트랜스폼입니다.
        public Slider m_AimSlider;                  // 현재 발사력을 표시하는 슬라이더 UI입니다.
        public AudioSource m_ShootingAudio;         // 발사 소리를 재생하는 오디오 소스입니다. (이동 오디오 소스와는 다름)
        public AudioClip m_ChargingClip;            // 발사를 충전할 때 재생되는 사운드입니다.
        public AudioClip m_FireClip;                // 포탄이 발사될 때 재생되는 사운드입니다.

        [Tooltip("최소 충전 시 발사 속도 (유닛/초)")]
        public float m_MinLaunchForce = 5f;        // 발사 버튼을 짧게 누를 때 적용되는 최소 발사력입니다.

        [Tooltip("최대 충전 시 발사 속도 (유닛/초)")]
        public float m_MaxLaunchForce = 20f;        // 최대 충전 시간까지 누를 경우 적용되는 최대 발사력입니다.

        [Tooltip("최대 충전 지속 시간. 이 시간을 넘기면 자동으로 최대 발사력으로 발사됩니다")]
        public float m_MaxChargeTime = 0.75f;       // 최대 충전 가능한 시간입니다.

        [Tooltip("발사 후 다시 발사 가능해질 때까지의 시간")]
        public float m_ShotCooldown = 1.0f;         // 두 번의 발사 사이에 필요한 쿨다운 시간입니다.

        [Header("Shell Properties")]
        [Tooltip("포탄 낙하 지점에 정확히 맞은 탱크가 입는 최대 피해량")]
        public float m_MaxDamage = 100f;                    // 폭발 중심에 있는 탱크에 가해지는 최대 데미지입니다.

        [Tooltip("폭발 중심에서의 폭발력 (단위: 뉴턴). 최소 500 이상 권장")]
        public float m_ExplosionForce = 1000f;              // 폭발 중심에 있는 탱크에 적용되는 폭발 힘입니다.

        [Tooltip("폭발 반경 (유닛). 중심에서 멀어질수록 피해 감소, 이 범위를 벗어난 탱크는 영향을 받지 않습니다")]
        public float m_ExplosionRadius = 5f;                // 폭발이 영향을 미치는 최대 거리입니다.

        [HideInInspector]
        public TankInputUser m_InputUser; // 이 탱크의 InputUser 컴포넌트로, 입력 액션들을 포함합니다.

        public float CurrentChargeRatio =>
            (m_CurrentLaunchForce - m_MinLaunchForce) / (m_MaxLaunchForce - m_MinLaunchForce); // 현재 충전 정도 (0~1)

        public bool IsCharging => m_IsCharging; // 현재 충전 중인지 여부

        public bool m_IsComputerControlled { get; set; } = false; // AI 제어 여부

        private string m_FireButton;                // 발사 입력 축 이름입니다.
        private float m_CurrentLaunchForce;         // 발사 시 적용될 현재 발사력입니다.
        private float m_ChargeSpeed;                // 발사력 증가 속도입니다. 최대 충전 시간 기반으로 계산됩니다.
        private bool m_Fired;                       // 포탄이 이미 발사되었는지를 나타냅니다.
        private bool m_HasSpecialShell;             // 특수 포탄이 있는지 여부입니다.
        private float m_SpecialShellMultiplier;     // 특수 포탄의 데미지 배수입니다.
        private InputAction fireAction;             // 발사용 Input Action입니다. TankInputUser에서 가져옵니다.
        private bool m_IsCharging = false;          // 현재 충전 중인지 여부입니다.
        private float m_BaseMinLaunchForce;         // 초기 최소 발사력입니다.
        private float m_ShotCooldownTimer;          // 발사 쿨다운 타이머입니다.

        private void OnEnable()
        {
            // 탱크가 활성화되면 발사력, 슬라이더, 특수 효과 등을 초기화합니다.
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_BaseMinLaunchForce = m_MinLaunchForce;
            m_AimSlider.value = m_BaseMinLaunchForce;
            m_HasSpecialShell = false;
            m_SpecialShellMultiplier = 1.0f;

            m_AimSlider.minValue = m_MinLaunchForce;
            m_AimSlider.maxValue = m_MaxLaunchForce;
        }

        private void Awake()
        {
            // InputUser 컴포넌트가 없으면 자동으로 추가합니다.
            m_InputUser = GetComponent<TankInputUser>();
            if (m_InputUser == null)
                m_InputUser = gameObject.AddComponent<TankInputUser>();
        }

        private void Start()
        {
            // 플레이어 번호에 기반한 발사 축 이름입니다.
            m_FireButton = "Fire";
            fireAction = m_InputUser.ActionAsset.FindAction(m_FireButton);
            fireAction.Enable();

            // 최대 발사 시간 기준으로 발사력 증가 속도를 계산합니다.
            m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
        }

        private void Update()
        {
            // AI 또는 플레이어 입력 처리 함수를 나눠 호출합니다.
            if (!m_IsComputerControlled)
            {
                HumanUpdate();
            }
            else
            {
                ComputerUpdate();
            }
        }

        /// <summary>
        /// AI가 충전을 시작할 때 호출합니다.
        /// </summary>
        public void StartCharging()
        {
            m_IsCharging = true;
            // 포탄 발사 여부 초기화 및 발사력 초기화
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;

            // 충전 사운드 재생
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }

        public void StopCharging()
        {
            if (m_IsCharging)
            {
                Fire();
                m_IsCharging = false;
            }
        }

        void ComputerUpdate()
        {
            // 슬라이더 초기화
            m_AimSlider.value = m_BaseMinLaunchForce;

            // 최대 충전 상태에서 아직 발사되지 않았다면 즉시 발사
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            // 충전 중이며 아직 발사되지 않았다면 충전 증가
            else if (m_IsCharging && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // 버튼이 떼어졌고 아직 발사되지 않았다면 발사
            else if (fireAction.WasReleasedThisFrame() && !m_Fired)
            {
                Fire();
                m_IsCharging = false;
            }
        }

        void HumanUpdate()
        {
            // 쿨다운 타이머 감소
            if (m_ShotCooldownTimer > 0.0f)
            {
                m_ShotCooldownTimer -= Time.deltaTime;
            }

            // 슬라이더 초기화
            m_AimSlider.value = m_BaseMinLaunchForce;

            // 최대 충전 상태에서 아직 발사되지 않았다면 즉시 발사
            if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
            {
                m_CurrentLaunchForce = m_MaxLaunchForce;
                Fire();
            }
            // 발사 버튼이 막 눌렸고 쿨다운이 끝났다면 충전 시작
            else if (m_ShotCooldownTimer <= 0 && fireAction.WasPressedThisFrame())
            {
                m_Fired = false;
                m_CurrentLaunchForce = m_MinLaunchForce;

                m_ShootingAudio.clip = m_ChargingClip;
                m_ShootingAudio.Play();
            }
            // 버튼이 계속 눌리고 있으며 아직 발사되지 않았다면 계속 충전
            else if (fireAction.IsPressed() && !m_Fired)
            {
                m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
                m_AimSlider.value = m_CurrentLaunchForce;
            }
            // 버튼이 떼어졌고 아직 발사되지 않았다면 발사
            else if (fireAction.WasReleasedThisFrame() && !m_Fired)
            {
                Fire();
            }
        }

        private void Fire()
        {
            // 이 프레임에서 한 번만 발사되도록 플래그 설정
            m_Fired = true;

            // 포탄 프리팹을 생성하고 Rigidbody 참조 획득
            Rigidbody shellInstance =
                Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

            // 현재 발사력만큼 정면 방향으로 속도 설정
            shellInstance.linearVelocity = m_CurrentLaunchForce * m_FireTransform.forward;

            // 폭발 속성 설정
            ShellExplosion explosionData = shellInstance.GetComponent<ShellExplosion>();
            explosionData.m_ExplosionForce = m_ExplosionForce;
            explosionData.m_ExplosionRadius = m_ExplosionRadius;
            explosionData.m_MaxDamage = m_MaxDamage;

            // 특수 포탄일 경우 추가 데미지 적용 및 초기화
            if (m_HasSpecialShell)
            {
                explosionData.m_MaxDamage *= m_SpecialShellMultiplier;
                m_HasSpecialShell = false;
                m_SpecialShellMultiplier = 1f;

                PowerUpDetector powerUpDetector = GetComponent<PowerUpDetector>();
                if (powerUpDetector != null)
                    powerUpDetector.m_HasActivePowerUp = false;

                PowerUpHUD powerUpHUD = GetComponentInChildren<PowerUpHUD>();
                if (powerUpHUD != null)
                    powerUpHUD.DisableActiveHUD();
            }

            // 발사 사운드 재생
            m_ShootingAudio.clip = m_FireClip;
            m_ShootingAudio.Play();

            // 발사력과 쿨다운 초기화
            m_CurrentLaunchForce = m_MinLaunchForce;
            m_ShotCooldownTimer = m_ShotCooldown;
        }

        public void EquipSpecialShell(float damageMultiplier)
        {
            // 외부에서 특수 포탄을 장착할 수 있게 합니다.
            m_HasSpecialShell = true;
            m_SpecialShellMultiplier = damageMultiplier;
        }

        /// <summary>
        /// 주어진 충전량(0~1)에 따라 포탄이 도달할 예상 위치를 반환합니다. (장애물 무시)
        /// </summary>
        public Vector3 GetProjectilePosition(float chargingLevel)
        {
            // 주어진 충전량을 통해 발사 속도 계산
            float chargeLevel = Mathf.Lerp(m_MinLaunchForce, m_MaxLaunchForce, chargingLevel);
            Vector3 velocity = chargeLevel * m_FireTransform.forward;

            // 수직 방향 낙하 시간 계산 (2차 방정식 이용)
            float a = 0.5f * Physics.gravity.y;
            float b = velocity.y;
            float c = m_FireTransform.position.y;

            float sqrtContent = b * b - 4 * a * c;
            if (sqrtContent <= 0)
            {
                return m_FireTransform.position;
            }

            float answer1 = (-b + Mathf.Sqrt(sqrtContent)) / (2 * a);
            float answer2 = (-b - Mathf.Sqrt(sqrtContent)) / (2 * a);

            float answer = answer1 > 0 ? answer1 : answer2;

            // 포탄이 바닥에 도달할 때까지의 수평 거리 계산
            Vector3 position = m_FireTransform.position +
                               new Vector3(velocity.x, 0, velocity.z) *
                               answer;
            position.y = 0;

            return position;
        }
    }
}
