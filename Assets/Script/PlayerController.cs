using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float moveSpeed = 6f;
    

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
}
