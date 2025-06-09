using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private Vector2 lastVelocity;
    private int hp = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, float speed, int hp)
    {
        this.speed = speed;
        this.hp = hp;
        rb.linearVelocity = direction.normalized * speed;
    }

    private void Update()
    {
        if (rb != null && rb.linearVelocity.sqrMagnitude > 0.001f)
        {
            lastVelocity = rb.linearVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        // 다른 총알과 충돌
        if (collision.collider.TryGetComponent<TestBullet>(out var otherBullet))
        {
            hp--;

            if (hp <= 0)
            {
                Destroy(gameObject);
                return;
            }

            // 반사 (속도 유지)
            rb.linearVelocity = lastVelocity.normalized * speed;
            return;
        }
        
        if (collision.contactCount == 0) return;

        Vector2 normal = collision.contacts[0].normal;
        Vector2 incoming = lastVelocity.normalized;
        Vector2 reflected = Vector2.Reflect(incoming, normal);

        rb.linearVelocity = reflected * speed;
        transform.position = collision.contacts[0].point + normal * 0.01f;
    }
}