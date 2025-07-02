using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private bool isDead;
    public bool playerUnlocked = true;

    [Header("Knockback Info")]
    [SerializeField] private Vector2 knockbackDir;
    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Move Info")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedMultiplier;
    private float defaultSpeed;
    [Space]
    [SerializeField] private float milestoneIncreaser;
    private float defaultMilestoneIncreaser;
    private float speedMilestone;



    [Header("Jump Info")]
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
        sr = GetComponent<SpriteRenderer>();

        speedMilestone = milestoneIncreaser;
        defaultSpeed = moveSpeed;
        defaultMilestoneIncreaser = milestoneIncreaser;
    }

    private void Update()
    {
        CheckCollision();
        AnimatorControllers();

        slideTimeCounter -= Time.deltaTime;
        slideCooldownCounter -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.K))
        {
            Knockback();
        }
        if (Input.GetKeyDown(KeyCode.D) && !isDead)
        {
            StartCoroutine(Die());
        }


        if (isDead) return;

        if (isKnocked) return;

        if (playerUnlocked && !wallDirected)
        {
            SetupMovement();
        }

        if(isGrounded)
        {
            canDoubleJump = true;
        }

        SpeedController();

        CheckForLedge();
        CheckForSlideCancel();
        CheckInput();
    }

    public void Damage()
    {
        if (moveSpeed >= maxSpeed)
        {
            Knockback();
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        canBeKnocked = false;
        rb.linearVelocity = knockbackDir;
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(0.7f);
        rb.linearVelocity = Vector2.zero;
    }

    private IEnumerator Invincibility()
    {
        Color orignalColor = sr.color;
        Color darkenColor = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);

        canBeKnocked = false;
        sr.color = darkenColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = orignalColor;
        yield return new WaitForSeconds(0.1f);
        sr.color = darkenColor;
        yield return new WaitForSeconds(0.15f);
        sr.color = orignalColor;
        yield return new WaitForSeconds(0.15f);
        sr.color = darkenColor;
        yield return new WaitForSeconds(0.25f);
        sr.color = orignalColor;
        yield return new WaitForSeconds(0.25f);
        sr.color = darkenColor;
        yield return new WaitForSeconds(0.3f);
        sr.color = orignalColor;


        canBeKnocked = true;
    }

    private void Knockback()
    {
        if(!canBeKnocked) return;

        StartCoroutine(Invincibility());
        isKnocked = true;
        rb.linearVelocity = knockbackDir;
    }

    private void CancelKnockback()
    {
        isKnocked = false;
        rb.linearVelocity = Vector2.zero;
        SpeedReset();
    }

    private void SpeedReset()
    {
        moveSpeed = defaultSpeed;
        speedMilestone = defaultMilestoneIncreaser;
    }

    private void SpeedController()
    {
        if (moveSpeed == maxSpeed) return;

        if (transform.position.x > speedMilestone)
        {
            speedMilestone = speedMilestone + milestoneIncreaser;

            moveSpeed *= speedMultiplier;
            milestoneIncreaser = milestoneIncreaser * speedMultiplier;

            if (moveSpeed > maxSpeed)
            {
                moveSpeed = maxSpeed;
            }
        }
    }

    private void CheckForLedge()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            rb.linearVelocity = Vector2.zero;         // 벽을 오를 때 속도를 0으로 초기화

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            canClimb = true;
        }

        if (canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }

    private void LedgeClimbOver()
    {
        canClimb = false;
        rb.gravityScale = 5.0f;
        transform.position = climbOverPosition;
        Invoke(nameof(AllowLedgeGrab), 0.1f);
    }

    private void AllowLedgeGrab() => canGrabLedge = true;
    

    private void CheckForSlideCancel()
    {
        if (slideTimeCounter < 0 && !ceillingDirected)
        {
            isSliding = false;
        }
    }

    private void SetupMovement()
    {
        if (wallDirected)
        {
            Debug.Log("Wall Detected");
            SpeedReset();
            return;
        }

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
        anim.SetBool("isKnocked", isKnocked);

        if (rb.linearVelocityY < -20)
        {
            anim.SetBool("canRoll", true);
        }
    }

    private void RollAnimFinished()
    {
        anim.SetBool("canRoll", false);
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
