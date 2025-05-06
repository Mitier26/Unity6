using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public Transform GunArm;
    
    private Camera mainCamera;
    
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 이동
        rb.linearVelocity = moveInput * moveSpeed;

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // 스프라이트 반전 (마우스가 왼쪽에 있으면 뒤집기)
        if (mouseWorldPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            GunArm.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            GunArm.localScale = new Vector3(1f, 1f, 1f);
        }

        // 방향 벡터 계산
        Vector3 lookDir = mouseWorldPos - GunArm.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // 무기 회전 적용 (GunArm이 오른쪽을 기준으로 만든 경우)
        GunArm.rotation = Quaternion.Euler(0, 0, angle);
        
        if(rb.linearVelocity != Vector2.zero)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }


    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}
