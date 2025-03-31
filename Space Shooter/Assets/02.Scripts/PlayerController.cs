using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    // 2D플레이어 이동 구현
    public float moveSpeed = 5.0f;
    Rigidbody2D rigid;

    [SerializeField] Transform bottomLeftLimit;
    [SerializeField] Transform topRightLimit;

    public Transform shotPoint;
    public GameObject shotPrefab;
    
    public float timeBetweenShots = 0.1f;
    private float shotCounter;

    private float normalSpeed;
    public float boostSpeed;
    public float boostLength;
    private float boostCounter;
    
    void Awake()
    {
        instance = this;
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        normalSpeed = moveSpeed;
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
            
            shotCounter = timeBetweenShots;
        }

        if (Input.GetButton("Fire1"))
        {
            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                Instantiate(shotPrefab, shotPoint.position, shotPoint.rotation);
            
                shotCounter = timeBetweenShots;
            }
        }

        if (boostCounter > 0)
        {
            boostCounter -= Time.deltaTime;
            if (boostCounter <= 0)
            {
                moveSpeed = normalSpeed;
            }
        }
    }

    public void ActivateSpeedBoost()
    {
        boostCounter = boostLength;
        moveSpeed = boostSpeed;
    }
}
