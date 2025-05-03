using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Tanks.Complete
{
    /// <summary>
    /// 컴퓨터 제어 탱크의 이동·사격 AI를 담당합니다.
    /// </summary>
    public class TankAI : MonoBehaviour
    {
        // AI 탱크의 상태값 : 목표 추적(Seek) 또는 회피(Flee)
        enum State
        {
            Seek, // 추적 중
            Flee  // 도망 중
        }

        private TankMovement m_Movement;                // 이동 제어 스크립트
        private TankShooting m_Shooting;                // 사격 제어 스크립트

        private float m_PathfindTime = 0.5f;            // 경로 탐색을 수행할 최소 간격(초)
        private float m_PathfindTimer = 0.0f;           // 다음 경로 탐색까지 남은 시간(초)

        private Transform m_CurrentTarget = null;       // 현재 추적 중인 대상 트랜스폼
        private float m_MaxShootingDistance = 0.0f;     // 최대 사거리(풀 차지 기준) - 사격 판단에 사용

        private float m_TimeBetweenShot = 2.0f;         // 연속 사격 제한(쿨다운) 시간(초)
        private float m_ShotCooldown = 0.0f;            // 다음 사격까지 남은 시간(초)

        private Vector3 m_LastTargetPosition;           // 직전 프레임의 대상 위치
        private float m_TimeSinceLastTargetMove;        // 대상이 정지한 누적 시간(초) – 일정 시간 이상 정지 시 회피 전환

        private NavMeshPath m_CurrentPath = null;       // 현재 따라가는 네브메시 경로
        private int m_CurrentCorner = 0;                // 경로 상에서 현재 향하는 코너 인덱스
        private bool m_IsMoving = false;                // 이동 중 여부(사격 시 정지)

        private GameObject[] m_AllTanks;                // 씬 내 존재하는 모든 탱크 오브젝트

        private State m_CurrentState = State.Seek;      // 현재 AI 상태값(기본값: 추적)

        //--------------------------------------------------
        // Unity 생명주기 메서드
        //--------------------------------------------------
        private void Awake()
        {
            // 컴포넌트가 비활성화된 경우 초기화 건너뛰기
            if (!isActiveAndEnabled)
                return;

            m_Movement = GetComponent<TankMovement>();
            m_Shooting = GetComponent<TankShooting>();

            // 이동·사격 스크립트를 AI 모드로 설정
            m_Movement.m_IsComputerControlled = true;
            m_Shooting.m_IsComputerControlled = true;

            // AI 탱크마다 경로 탐색 간격을 랜덤화하여 CPU 부하 분산
            m_PathfindTime = Random.Range(0.3f, 0.6f);

            // 풀 차지(1.0f) 시 투사체가 도달할 수 있는 최대 거리 계산
            m_MaxShootingDistance = Vector3.Distance(
                m_Shooting.GetProjectilePosition(1.0f),
                transform.position);

            // GameManager 없이도 동작하도록 씬의 모든 탱크 탐색
            m_AllTanks = FindObjectsByType<TankMovement>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                .Select(t => t.gameObject).ToArray();
        }

        // GameManager가 있는 씬에서 호출되어 AI가 사용할 탱크 목록을 교체함
        public void Setup(GameManager manager)
        {
            m_AllTanks = manager.m_SpawnPoints.Select(s => s.m_Instance).ToArray();
        }

        // 외부에서 AI를 끌 때 호출
        public void TurnOff()
        {
            enabled = false;
        }

        //--------------------------------------------------
        // 매 프레임 호출 – 상태 머신 업데이트
        //--------------------------------------------------
        void Update()
        {
            // 사격 쿨다운 감소
            if (m_ShotCooldown > 0)
                m_ShotCooldown -= Time.deltaTime;

            // 경로 탐색 타이머 증가
            m_PathfindTimer += Time.deltaTime;

            // 현재 상태별 로직 실행
            switch (m_CurrentState)
            {
                case State.Seek:
                    SeekUpdate();
                    break;
                case State.Flee:
                    FleeUpdate();
                    break;
            }
        }

        //--------------------------------------------------
        // 추적 상태 로직
        //--------------------------------------------------
        void SeekUpdate()
        {
            // 일정 주기마다만 경로 탐색하여 성능 최적화
            if (m_PathfindTimer > m_PathfindTime)
            {
                m_PathfindTimer = 0;

                // 각 탱크까지의 경로 계산
                NavMeshPath[] paths = new NavMeshPath[m_AllTanks.Length];
                float shortestPath = float.MaxValue;
                int bestIndex = -1;
                Transform bestTarget = null;

                for (int i = 0; i < m_AllTanks.Length; i++)
                {
                    GameObject tank = m_AllTanks[i];
                    if (tank == gameObject) continue;    // 자신 제외
                    if (tank == null || !tank.activeInHierarchy) continue; // 파괴·비활성 제외

                    paths[i] = new NavMeshPath();
                    if (NavMesh.CalculatePath(transform.position, tank.transform.position, NavMesh.AllAreas, paths[i]))
                    {
                        float len = GetPathLength(paths[i]);
                        if (len < shortestPath)
                        {
                            shortestPath = len;
                            bestIndex = i;
                            bestTarget = tank.transform;
                        }
                    }
                }

                // 최단 경로 대상 설정
                if (bestIndex != -1)
                {
                    if (bestTarget != m_CurrentTarget)
                    {
                        m_CurrentTarget = bestTarget;
                        m_LastTargetPosition = m_CurrentTarget.position;
                    }

                    m_CurrentPath = paths[bestIndex];
                    m_CurrentCorner = 1;
                    m_IsMoving = true;
                }
            }

            // 추적 & 사격 판단
            if (m_CurrentTarget == null) return;

            // 대상 이동량 측정
            float movement = Vector3.Distance(m_CurrentTarget.position, m_LastTargetPosition);
            if (movement < 0.0001f)
                m_TimeSinceLastTargetMove += Time.deltaTime;
            else
                m_TimeSinceLastTargetMove = 0;
            m_LastTargetPosition = m_CurrentTarget.position;

            // 대상 방향·거리 계산
            Vector3 toTarget = m_CurrentTarget.position - transform.position;
            toTarget.y = 0;
            float targetDist = toTarget.magnitude;
            toTarget.Normalize();
            float facingDot = Vector3.Dot(toTarget, transform.forward);

            // 사격 중이라면 – 현재 차지로 명중 가능 여부 확인
            if (m_Shooting.IsCharging)
            {
                Vector3 projPos = m_Shooting.GetProjectilePosition(m_Shooting.CurrentChargeRatio);
                float shotDist = Vector3.Distance(projPos, transform.position);
                if (shotDist >= targetDist - 2 && facingDot > 0.99f)
                {
                    m_IsMoving = false;
                    m_Shooting.StopCharging();
                    m_ShotCooldown = m_TimeBetweenShot;

                    // 상대도 정지 상태라면 회피 모드 진입
                    if (m_TimeSinceLastTargetMove > 2.0f)
                        StartFleeing();
                }
            }
            // 아직 차지 전이라면 – 사거리·시야 확보 시 차지 시작
            else if (targetDist < m_MaxShootingDistance)
            {
                if (!NavMesh.Raycast(transform.position, m_CurrentTarget.position, out _, NavMesh.AllAreas))
                {
                    m_IsMoving = false;
                    if (m_ShotCooldown <= 0)
                        m_Shooting.StartCharging();
                }
            }
        }

        //--------------------------------------------------
        // 도망 상태 로직
        //--------------------------------------------------
        void FleeUpdate()
        {
            // 경로 마지막 코너에 도달하면 다시 추적 모드로 전환
            if (m_CurrentCorner >= m_CurrentPath.corners.Length)
                m_CurrentState = State.Seek;
        }

        // 회피 경로 생성
        void StartFleeing()
        {
            Vector3 dir = (m_CurrentTarget.position - transform.position).normalized;
            // 90~180도 랜덤 회전으로 반대 방향 벡터 생성
            dir = Quaternion.AngleAxis(Random.Range(90f, 180f) * Mathf.Sign(Random.Range(-1f, 1f)), Vector3.up) * dir;
            dir *= Random.Range(5f, 20f); // 5~20유닛 거리 목표

            if (NavMesh.CalculatePath(transform.position, transform.position + dir, NavMesh.AllAreas, m_CurrentPath))
            {
                m_CurrentState = State.Flee;
                m_CurrentCorner = 1;
                m_IsMoving = true;
            }
        }

        //--------------------------------------------------
        // FixedUpdate : 물리 기반 이동·회전 처리
        //--------------------------------------------------
        void FixedUpdate()
        {
            if (m_CurrentPath == null || m_CurrentPath.corners.Length == 0) return;

            Rigidbody rb = m_Movement.Rigidbody;

            // 이동·회전 목표 지점 설정
            Vector3 orientTarget = m_CurrentPath.corners[Mathf.Min(m_CurrentCorner, m_CurrentPath.corners.Length - 1)];
            if (!m_IsMoving) orientTarget = m_CurrentTarget.position;

            Vector3 toOrient = orientTarget - transform.position; toOrient.y = 0; toOrient.Normalize();
            Vector3 forward = rb.rotation * Vector3.forward;

            // 이동
            float move = Mathf.Clamp01(Vector3.Dot(forward, toOrient)) * m_Movement.m_Speed * Time.deltaTime;
            if (m_IsMoving && move > 1e-6f)
                rb.MovePosition(rb.position + forward * move);

            // 회전
            float angle = Vector3.SignedAngle(toOrient, forward, Vector3.up);
            angle = Mathf.Sign(angle) * Mathf.Min(Mathf.Abs(angle), m_Movement.m_TurnSpeed * Time.deltaTime);
            if (Mathf.Abs(angle) > 1e-6f)
                rb.MoveRotation(rb.rotation * Quaternion.AngleAxis(-angle, Vector3.up));

            // 코너 도달 체크
            if (Vector3.Distance(rb.position, orientTarget) < 0.5f)
                m_CurrentCorner++;
        }

        //--------------------------------------------------
        // 경로 길이 합산 유틸리티
        //--------------------------------------------------
        float GetPathLength(NavMeshPath path)
        {
            float length = 0f;
            for (int i = 1; i < path.corners.Length; i++)
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            return length;
        }
    }
}
