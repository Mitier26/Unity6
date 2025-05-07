using System;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    
    public float speed = 7.5f;

    [SerializeField] private GameObject impactEffect;

    public int damageToGive = 50;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            // 벽에 충돌했을 때 이펙트 생성
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Enemy"))
        {
            // 적에 충돌했을 때 이펙트 생성
            Instantiate(impactEffect, transform.position, Quaternion.identity);
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
            Destroy(gameObject);
        }
    }

    // 화면에서 벗어났을 때 총알 삭제
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
