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
        if(groundTouched.Count > 0)
        {
            JumpReset();
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
        if (actuallJumpHeight < maxJumpHeight && myRigidBody.velocity.y >= 0)
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
