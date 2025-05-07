using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator animator;
    
    public float moveSpeed;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public int health = 150;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
        {
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
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
