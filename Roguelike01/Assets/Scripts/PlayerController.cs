using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public Transform GunArm;
    
    private Camera mainCamera;
    
    private Animator animator;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    public float timeBetweenShots;
    private float shotCounter;
    
    public SpriteRenderer bodySprite;

    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = 0.5f, dashCooldown = 1f, dashInvincibility = 0.5f;
    
    [HideInInspector]
    public float dashCounter;
    private float dashcooldownCounter;

    [HideInInspector]
    public bool canMove = true;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        activeMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {
            // 이동
            rb.linearVelocity = moveInput * activeMoveSpeed;

            // 마우스 위치를 월드 좌표로 변환
            Vector3 mouseWorldPos = Input.mousePosition;
            mouseWorldPos.z = 0;
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.localPosition);

            // 스프라이트 반전 (마우스가 왼쪽에 있으면 뒤집기)
            if (mouseWorldPos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                GunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                GunArm.localScale = new Vector3(1f, 1f, 1f);
            }

            // 방향 벡터 계산
            Vector3 lookDir = new Vector2(mouseWorldPos.x - screenPoint.x, mouseWorldPos.y - screenPoint.y);
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            // 무기 회전 적용 (GunArm이 오른쪽을 기준으로 만든 경우)
            GunArm.rotation = Quaternion.Euler(0, 0, angle);

            // 총알 발사 마우스 왼쪽
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
                shotCounter = timeBetweenShots;
            }

            if (Input.GetMouseButton(0))
            {
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    Shoot();
                    shotCounter = timeBetweenShots;
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashcooldownCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    animator.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);

                    AudioManager.instance.PlaySfx(8);
                }
            }

            // 이동 속도 설정
            if (dashCounter > 0)
            {
                activeMoveSpeed = dashSpeed;
            }
            else
            {
                activeMoveSpeed = moveSpeed;
            }

            // 이동 애니메이션
            animator.SetBool("isMoving", rb.linearVelocity != Vector2.zero);

            // 플레이어가 대시 중이 아닐 때만 이동 가능
            if (dashCounter <= 0)
            {
                rb.linearVelocity = moveInput * activeMoveSpeed;
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashcooldownCounter = dashCooldown;
                }
            }

            if (dashcooldownCounter > 0)
            {
                dashcooldownCounter -= Time.deltaTime;
            }

            if (rb.linearVelocity != Vector2.zero)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);
        }
    }

    private void Shoot()
        {
            AudioManager.instance.PlaySfx(12);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>().normalized;
        }
    
}
