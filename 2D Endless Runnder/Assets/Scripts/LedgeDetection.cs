using UnityEngine;
using UnityEngine.UIElements;

public class LedgeDetection : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Player player;
    public bool ledgeDetected;

    private bool canDetected;

    private BoxCollider2D boxCd => GetComponent<BoxCollider2D>();

    private void Update()
    {
        if (canDetected)
            player.ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);

        if (canDetected)
        ledgeDetected = Physics2D.OverlapCircle(transform.position, radius, whatIsGround);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            canDetected = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCd.bounds.center, boxCd.size, 0);

        foreach (var hit in colliders)
        {
            if(hit.gameObject.GetComponent<PlatformController>() != null)
            {
                return;
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                canDetected = true;
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
