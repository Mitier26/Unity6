using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public bool playerUnlocked = true;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;


    [Header("Ground Check")]
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorControllers();

        if (playerUnlocked)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }

        CheckCollision();
        CheckInput();
    }

    private void AnimatorControllers()
    {
        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            playerUnlocked = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }
    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            canDoubleJump = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
        else if(canDoubleJump){
            canDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}
