using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirectionsCheck), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float airMoveSpeed = 3f;
    [SerializeField] private float jumpForce = 10;
    Vector2 moveInput;
    TouchingDirectionsCheck touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving
                    && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
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
                        //air move
                        return airMoveSpeed;
                    }
                }
                else
                {
                    return 0; //idle speed is 0
                }

            }
            else
            {
                return 0; //movement lock
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

    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return anim.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D playerRb;
    Animator anim;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirectionsCheck>();
        damageable = GetComponent<Damageable>();
    }

 

    private void FixedUpdate()
    {
        if (!damageable.IsHit)
        {
        playerRb.velocity = new(moveInput.x*CurrentMoveSpeed, playerRb.velocity.y);
        }

        anim.SetFloat(AnimationStrings.yVelocity, playerRb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

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
        if (context.started
            && touchingDirections.IsGrounded
            && CanMove)
        {
            anim.SetTrigger(AnimationStrings.jumpTrigger);
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit (int damage, Vector2 knockback)
    {
        playerRb.velocity = new Vector2(knockback.x, playerRb.velocity.y + knockback.y);
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }
}
