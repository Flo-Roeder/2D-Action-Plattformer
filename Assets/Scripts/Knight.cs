using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirectionsCheck))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.005f;
    public DetectionZone attackZone;

    Rigidbody2D rb;
    TouchingDirectionsCheck touchingDirectionsCheck;
    Animator anim;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            if (_walkDirection!=value)
            {
                // flip with local sclae
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x* -1, gameObject.transform.localScale.y);

                if (value== WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value==WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }

    private bool _hasTarget;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget= value;
            anim.SetBool(AnimationStrings.hasTarget,value);
        }
    }

    public bool CanMove 
    { get
        {
            return anim.GetBool(AnimationStrings.canMove);
        } 
    }


    // Start is called before the first frame update
    void Awake()
    {
        rb= GetComponent<Rigidbody2D>();
        touchingDirectionsCheck= GetComponent<TouchingDirectionsCheck>();
        anim= GetComponent<Animator>();
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (touchingDirectionsCheck.IsOnWall
            && touchingDirectionsCheck.IsGrounded)
        {
            FlipDirection();
        }
        if (CanMove)
        {
            rb.velocity= new Vector2(walkSpeed*walkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity= new Vector2(Mathf.Lerp(rb.velocity.x,0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection==WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection==WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("no valid direction set");
        }
    }
}
