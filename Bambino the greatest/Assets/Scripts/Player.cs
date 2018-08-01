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
        if (Input.GetKey(KeyCode.R))
        {
            Reset();
        }
        Move(Input.GetAxisRaw("Horizontal"));
    }

    private void LateUpdate()
    {
    }

    private void Move(float input)
    {
        float movement = 0;
        if (input != 0)
        {
            float actuallVelocityMagnitude = Math.Abs(myRigidBody.velocity.x);
            if (actuallVelocityMagnitude < maxMovementVelocity ||
                (!IsGrounded() && Math.Sign(myRigidBody.velocity.x) != Math.Sign(input)))
            {
                float movementPowerDivider = actuallVelocityMagnitude < 1 ? 1f : actuallVelocityMagnitude / 3;
                movement = input * groundMovementVelocity / movementPowerDivider;
                myRigidBody.AddForce(new Vector2(movement, 0));
            }
        }
        else
        {
            if (IsGrounded()) myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
        }

    }

    private void Jump()
    {
        if (actuallJumpHeight < maxJumpHeight)
        {
            actuallJumpHeight += 1;
            myRigidBody.AddForce(new Vector2(0f, jumpYVelocity / actuallJumpHeight));
        }
    }


    private bool IsGrounded()
    {
        if (actuallJumpHeight > 0) return false;
        else return true;
    }

    private void Reset()
    {
        actuallJumpHeight = 0;
        myRigidBody.gravityScale = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
