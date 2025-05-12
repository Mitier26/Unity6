using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator animator;
    
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public int health = 150;
    
    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldShoot;
    
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public float shootRange;
    public SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 적이 화면 안에 있는지 확인
        if (spriteRenderer.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            // 플레이어와의 거리 계산
            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {
                // 플레이어를 추적
                moveDirection = (PlayerController.instance.transform.position - transform.position).normalized;
                rb.linearVelocity = moveDirection * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
            
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
            Destroy(gameObject);
        }
    }
}
