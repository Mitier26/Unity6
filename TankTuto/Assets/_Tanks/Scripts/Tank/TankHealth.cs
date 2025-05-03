using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Complete
{
    public class TankHealth : MonoBehaviour
    {
        public float m_StartingHealth = 100f;               // 각 탱크가 시작할 때의 체력입니다.
        public Slider m_Slider;                             // 현재 체력을 나타내는 UI 슬라이더입니다.
        public Image m_FillImage;                           // 슬라이더의 채움 이미지입니다.
        public Color m_FullHealthColor = Color.green;       // 체력이 가득 찼을 때의 색상입니다.
        public Color m_ZeroHealthColor = Color.red;         // 체력이 0일 때의 색상입니다.
        public GameObject m_ExplosionPrefab;                // 탱크가 죽을 때 사용할 폭발 이펙트 프리팹입니다.
        [HideInInspector] public bool m_HasShield;          // 탱크가 실드 파워업을 획득했는지 여부입니다.

        private AudioSource m_ExplosionAudio;               // 폭발 시 재생될 오디오 소스입니다.
        private ParticleSystem m_ExplosionParticles;        // 탱크 파괴 시 재생될 파티클 시스템입니다.
        private float m_CurrentHealth;                      // 현재 체력입니다.
        private bool m_Dead;                                // 이미 죽었는지를 나타냅니다.
        private float m_ShieldValue;                        // 실드가 있을 경우 감소되는 데미지 비율입니다.
        private bool m_IsInvincible;                        // 현재 무적 상태인지 여부입니다.

        private void Awake()
        {
            // 폭발 이펙트를 인스턴스화하고 파티클 시스템 참조를 얻습니다.
            m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();

            // 오디오 소스도 참조합니다.
            m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

            // 초기에 비활성화 해두었다가 죽을 때 사용합니다.
            m_ExplosionParticles.gameObject.SetActive(false);

            // 슬라이더의 최대 값을 시작 체력으로 설정합니다.
            m_Slider.maxValue = m_StartingHealth;
        }

        private void OnDestroy()
        {
            // 파티클 객체 정리
            if (m_ExplosionParticles != null)
                Destroy(m_ExplosionParticles.gameObject);
        }

        private void OnEnable()
        {
            // 탱크가 다시 활성화될 때 상태 초기화
            m_CurrentHealth = m_StartingHealth;
            m_Dead = false;
            m_HasShield = false;
            m_ShieldValue = 0;
            m_IsInvincible = false;

            // 체력 UI 갱신
            SetHealthUI();
        }

        public void TakeDamage(float amount)
        {
            // 무적 상태가 아닐 경우에만 데미지를 입습니다.
            if (!m_IsInvincible)
            {
                // 실드 비율을 적용한 데미지를 체력에서 감소시킵니다.
                m_CurrentHealth -= amount * (1 - m_ShieldValue);

                // 체력 UI 업데이트
                SetHealthUI();

                // 체력이 0 이하이고 아직 죽지 않았다면 사망 처리
                if (m_CurrentHealth <= 0f && !m_Dead)
                {
                    OnDeath();
                }
            }
        }

        public void IncreaseHealth(float amount)
        {
            // 최대 체력을 넘지 않도록 제한
            if (m_CurrentHealth + amount <= m_StartingHealth)
            {
                m_CurrentHealth += amount;
            }
            else
            {
                m_CurrentHealth = m_StartingHealth;
            }

            // 체력 UI 갱신
            SetHealthUI();
        }

        public void ToggleShield(float shieldAmount)
        {
            // 실드 여부 토글
            m_HasShield = !m_HasShield;

            // 실드 적용 시 데미지 감소 비율 설정
            if (m_HasShield)
            {
                m_ShieldValue = shieldAmount;
            }
            else
            {
                m_ShieldValue = 0;
            }
        }

        public void ToggleInvincibility()
        {
            // 무적 상태 토글
            m_IsInvincible = !m_IsInvincible;
        }

        private void SetHealthUI()
        {
            // 슬라이더 값 설정
            m_Slider.value = m_CurrentHealth;

            // 현재 체력 비율에 따라 색상 보간
            m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
        }

        private void OnDeath()
        {
            // 중복 호출 방지
            m_Dead = true;

            // 폭발 이펙트를 탱크 위치로 이동 후 활성화
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            // 파티클과 사운드 재생
            m_ExplosionParticles.Play();
            m_ExplosionAudio.Play();

            // 탱크 비활성화
            gameObject.SetActive(false);
        }
    }
}
