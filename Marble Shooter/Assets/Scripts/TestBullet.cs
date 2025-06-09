using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private float speed;
    private Vector2 lastVelocity;
    private int hp = 1;

    private HashSet<Collider2D> hitCells = new HashSet<Collider2D>();
    private bool hasProcessedCellHitThisFrame = false;

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

    private void LateUpdate()
    {
        hasProcessedCellHitThisFrame = false; // 프레임마다 셀 충돌 처리 플래그 초기화
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        // 다른 총알과 충돌
        if (other.TryGetComponent<TestBullet>(out var otherBullet))
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
                return;
            }

            // 반사
            rb.linearVelocity = lastVelocity.normalized * speed;
            return;
        }

        // 셀과 충돌
        if (other.TryGetComponent<CellController>(out var cell))
        {
            if (other.layer != gameObject.layer) // 적 팀 셀만 반응
            {
                if (!hitCells.Contains(collision.collider) && !hasProcessedCellHitThisFrame)
                {
                    hitCells.Add(collision.collider);
                    hasProcessedCellHitThisFrame = true;

                    cell.HitByBullet(gameObject.layer);
                    hp--;

                    if (hp <= 0)
                    {
                        Destroy(gameObject);
                        return;
                    }
                }
            }
            return;
        }

        // 벽 등 반사 처리
        if (collision.contactCount == 0) return;

        Vector2 normal = collision.contacts[0].normal;
        Vector2 incoming = lastVelocity.normalized;
        Vector2 reflected = Vector2.Reflect(incoming, normal);

        rb.linearVelocity = reflected * speed;
        transform.position = collision.contacts[0].point + normal * 0.01f;
    }
}
