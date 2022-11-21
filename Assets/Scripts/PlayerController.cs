using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirectionsCheck))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] private float jumpForce=10;
    Vector2 moveInput;
    TouchingDirectionsCheck touchingDirections;

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                return 0; //idle speed is 0
            }
        }
    }

    private bool _isMoving;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            anim.SetBool(AnimationStrings.isMoving, value);
        }
    }

    private bool _isRunning;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            anim.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private bool _isFacingRight=true;
    public bool IsFacingRight 
    {
        get
        {
            return _isFacingRight;
        }
        private set 
        {
            if (_isFacingRight!=value) //flip with local scale
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    Rigidbody2D playerRb;
    Animator anim;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirectionsCheck>();
    }

 

    private void FixedUpdate()
    {
        playerRb.velocity = new(moveInput.x*CurrentMoveSpeed, playerRb.velocity.y);

        anim.SetFloat(AnimationStrings.yVelocity, playerRb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x>0
            && !IsFacingRight) //face right
        {
            IsFacingRight = true;
        }
        else if (moveInput.x<0
            && IsFacingRight) // face left
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //TODO check if is alive
        if (context.started
            && touchingDirections.IsGrounded)
        {
            anim.SetTrigger(AnimationStrings.jump);
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
    }
}
