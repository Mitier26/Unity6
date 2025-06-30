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
    [SerializeField] private float ceillingCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Vector2 wallCheckSize;

    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool wallDirected;
    private bool ceillingDirected;
    [HideInInspector] public bool ledgeDetected;

    [Header("Ledge Info")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;
    
    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;



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

        CheckForLedge();
        CheckForSlide();
        CheckInput();
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if(canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        transform.position = climbOverPosition;
        Invoke(nameof(AllowLedgeGrab), 0.1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;
    

    private void CheckForSlide()
    {
        if (slideTimeCounter < 0 && !ceillingDirected)
        {
            isSliding = false;
        }
    }

    private void Movement()
    {
        if (wallDirected) return;

        if (isSliding)
        {
            rb.linearVelocity = new Vector2(slideSpeed, rb.linearVelocityY);
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
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
        if (isSliding) return;

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

    private void AnimatorControllers()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);

        anim.SetBool("canDoubleJump", canDoubleJump);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
        anim.SetBool("canClimb", canClimb);
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        ceillingDirected = Physics2D.Raycast(transform.position, Vector2.up, ceillingCheckDistance, whatIsGround);
        wallDirected = Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0f, Vector2.zero, 0f, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingCheckDistance));
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
    }
}
