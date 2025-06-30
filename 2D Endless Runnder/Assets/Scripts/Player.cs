using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public bool playerUnlocked = true;


    [Header("Movement Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float doubleJumpForce;
    private bool canDoubleJump;

        [Header("Slide Info")]
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slideTimer;
    [SerializeField] private float slideCooldown;
    private float slideCooldownCounter;
    private float slideTimeCounter;
    private bool isSliding;


    [Header("Collision Check")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool wallDirected;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckCollision();
        AnimatorControllers();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if (playerUnlocked && !wallDirected)
        {
            Movement();
        }

        if(isGrounded)
        {
            canDoubleJump = true;
        }

        CheckForSlide();
        CheckInput();
    }

    private void CheckForSlide()
    {
        if (slideTimeCounter < 0)
        {
            isSliding = false;
        }
    }

    private void Movement()
    {
        if (isSliding)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }
    }

    private void AnimatorControllers()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDirected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.zero, 0f, whatIsGround);
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SlideButton();
        }
    }

    private void SlideButton()
    {
        if (rb.linearVelocityX != 0 && slideCooldownCounter < 0)
        {
            isSliding = true;
            slideTimeCounter = slideTimer;
            slideCooldownCounter = slideCooldown;
        }
    }

    private void JumpButton()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, doubleJumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
