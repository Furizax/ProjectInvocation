using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 4f;
    public bool isJumping = false;
    

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
        if(moveInput != 0f)
            lastMoveInput = moveInput;

        if(Input.GetButtonDown("Jump") && !isJumping)
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

    void Move()
    {
        
    }

    void Jump()
    {

    }
    
    void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight || moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x = 0.3538062f; 
            transform.localScale = ls;

        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false; 
    }
}
