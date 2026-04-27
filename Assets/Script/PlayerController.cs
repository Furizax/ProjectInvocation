using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 4f;
    public bool isJumping = true;


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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
    }

}
