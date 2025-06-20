using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator animator;
    
    public float moveSpeed;
    
    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    [Header("Run Away")]
    public bool shouldRunAway;
    public float runawayRange;

    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    public int health = 150;
    
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldShoot;
    
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public float shootRange;
    public SpriteRenderer spriteRenderer;
    
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    private bool IsInCameraView()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1 &&
               viewportPos.z > 0; // 카메라 앞에 있는지
    }

    private void Start()
    {
        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
        }
    }

    private void Update()
    {
        // 적이 화면 안에 있는지 확인
        if (IsInCameraView() && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;
            
            // 플레이어와의 거리 계산
            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                // 플레이어를 추적
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                if (shouldWander)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * 0.75f, pauseLength * 1.25f);
                        }
                    }

                    if (pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if (pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * 0.75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }
            }

            if (shouldPatrol)
            {
                moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < 0.2f)
                {
                    currentPatrolPoint++;
                    if (currentPatrolPoint >= patrolPoints.Length)
                    {
                        currentPatrolPoint = 0;
                    }
                }
            }

            // 도망가는 적
            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <
                rangeToChasePlayer)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }
            
            
            
            /*else
            {
                rb.linearVelocity = Vector2.zero;
            }*/
            
            moveDirection.Normalize();

            rb.linearVelocity = moveDirection * moveSpeed;
            
            // 총알 발사
            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;
                if (fireCounter <= 0)
                {
                    Shoot();
                    fireCounter = fireRate;
                }
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
        
        
        // 애니메이션 설정
        if (rb.linearVelocity.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        AudioManager.instance.PlaySfx(13);
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        AudioManager.instance.PlaySfx(2);
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        
        if (health <= 0)
        {
            AudioManager.instance.PlaySfx(1);
            // 적이 죽었을 때 흔적 생성
            int randomIndex = Random.Range(0, deathSplatters.Length);
            int randomRotation = Random.Range(0, 360);
            Instantiate(deathSplatters[randomIndex], transform.position, Quaternion.Euler(0, 0, randomRotation));
            
            // Drop item
            if (shouldDropItem)
            {
                float randomValue = Random.Range(0f, 100f);
                if (randomValue <= itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);
                    Instantiate(itemsToDrop[randomItem], transform.position, Quaternion.identity);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
