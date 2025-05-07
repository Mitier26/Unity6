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
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
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

    public void DamageEnemy(int damage)
    {
        health -= damage;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            // 적이 죽었을 때 흔적 생성
            int randomIndex = Random.Range(0, deathSplatters.Length);
            int randomRotation = Random.Range(0, 360);
            Instantiate(deathSplatters[randomIndex], transform.position, Quaternion.Euler(0, 0, randomRotation));
            Destroy(gameObject);
        }
    }
}
