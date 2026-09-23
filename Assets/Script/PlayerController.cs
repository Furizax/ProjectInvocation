using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    [Header("Value")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 6f;
    [Header("Detection")]
    public bool isJumping = true;
    public bool isGrounded = true;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    
    private float moveInput;
    private float lastMoveInput = 1f;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }

        GroundCheck();
        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
        
            Vector2 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            isFacingRight = !isFacingRight;
        }

    }

    void GroundCheck() // Empêche le joueur de sauter s'il est dans le vide et qu'il n'a pas sauté
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if(!isGrounded)
        {
            isJumping = true;
        }
        else
        { isJumping = false; }
    }

    private void OnCollisionEnter2D(Collision2D collision) //Pourquoi lol
    {
        isJumping = false;
    }



}
