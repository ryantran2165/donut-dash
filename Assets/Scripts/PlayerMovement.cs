﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1000f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private Transform groundTransform;
    [SerializeField] private LayerMask whatIsGround;
    private Rigidbody2D rigidBody;
    private bool facingRight = true;
    private bool isGrounded = true;
    private bool jump;
    private float horizontalMove;
    private Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        movement = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        // Poll input
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }

        // Cast circle to check if grounded
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundTransform.position, .5f, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
            }
        }
    }

    private void FixedUpdate()
    {
        // Move horizontal
        movement.Set(horizontalMove * Time.fixedDeltaTime, 0);
        rigidBody.AddForce(movement);

        // Clamp velocity
        if (rigidBody.velocity.x > maxSpeed)
        {
            rigidBody.velocity = new Vector2(maxSpeed, rigidBody.velocity.y);
        }
        else if (rigidBody.velocity.x < -maxSpeed)
        {
            rigidBody.velocity = new Vector2(-maxSpeed, rigidBody.velocity.y);
        }

        //Flip sprite horizontal
        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 flippedScale = transform.localScale;
            flippedScale.x *= -1;
            transform.localScale = flippedScale;
        }

        // Jump
        if (isGrounded && jump)
        {
            isGrounded = false;
            rigidBody.AddForce(Vector2.up * jumpForce);
        }
        jump = false;
    }

    public void changeMaxSpeed(float dMaxSpeed)
    {
        maxSpeed += dMaxSpeed;
    }

}
