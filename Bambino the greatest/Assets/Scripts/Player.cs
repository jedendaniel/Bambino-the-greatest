using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    Rigidbody2D myRigidBody;

    public float jumpYVelocity = 120;
    public float maxJumpHeight = 7;
    float actuallJumpHeight = 0;
    public float fallMultiplier = 2.5f;

    public float groundMovementVelocity;
    public float airMovementVelocity;

    float actuallMovementVelocity = 0;
    public float movementRatioPerFrame;
    public float maxMovementVelocity;

    bool jumpRequest = false;
    List<Collider2D> groundTouched = new List<Collider2D>();

    private float horizontalInput = 0;


    void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            jumpRequest = true;
        }
        if(IsGrounded())
        {
            JumpReset();
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        if (myRigidBody.velocity.y < 0)
        {
            myRigidBody.gravityScale = fallMultiplier;
        }

        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }
        Move(horizontalInput);
    }

    private void Move(float input)
    {
        if(input == 0)
        {
            if (IsGrounded())
            {
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
            }
        }
        else
        {
            if (Math.Abs(myRigidBody.velocity.x) < groundMovementVelocity || Math.Sign(myRigidBody.velocity.x) != Math.Sign(input))
                myRigidBody.AddForce(new Vector2(input * airMovementVelocity, 0));

        }
    }

    private void Jump()
    {
        if (actuallJumpHeight < maxJumpHeight && myRigidBody.velocity.y >= 0)
        {
            actuallJumpHeight += 1;
            myRigidBody.AddForce(new Vector2(0f, jumpYVelocity / actuallJumpHeight));
        }
    }


    private bool IsGrounded()
    {
        return groundTouched.Count > 0;
    }

    private void JumpReset()
    {
        actuallJumpHeight = 0;
        myRigidBody.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contactPoint in collision.contacts){
            if (contactPoint.normal == Vector2.up && !groundTouched.Contains(collision.collider))
            {
                groundTouched.Add(collision.collider);
                JumpReset();
                return;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (groundTouched.Contains(collision.collider))
            groundTouched.Remove(collision.collider);
    }
}
