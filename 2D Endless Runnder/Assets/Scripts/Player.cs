using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool runBegun;
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;

    public Rigidbody2D rb;

    [Header("Ground Check")]
    private bool isGrounded;
    public float groundCheckDistance = 0.1f;
    public LayerMask whatIsGround;

    private void Update()
    {
        if (runBegun)
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocityY);
        }

        CheckCollision();
        CheckInput();
    }

    private void CheckCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            runBegun = true;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}
