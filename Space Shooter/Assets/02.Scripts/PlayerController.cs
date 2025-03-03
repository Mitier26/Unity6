using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 2D플레이어 이동 구현
    public float moveSpeed = 5.0f;
    Rigidbody2D rigid;

    [SerializeField] Transform bottomLeftLimit;
    [SerializeField] Transform topRightLimit;

    public Transform shotPoint;
    public GameObject shotPrefab;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 플레이어 이동
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);
        rigid.linearVelocity = dir * moveSpeed;

        // 플레이어 이동 제한
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, bottomLeftLimit.position.x + transform.localScale.x / 2, topRightLimit.position.x - transform.localScale.x / 2);
        pos.y = Mathf.Clamp(pos.y, bottomLeftLimit.position.y + transform.localScale.y / 2  , topRightLimit.position.y - transform.localScale.y / 2);
        transform.position = pos;

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(shotPrefab, shotPoint.position, shotPoint.rotation);
        }

    }
}
