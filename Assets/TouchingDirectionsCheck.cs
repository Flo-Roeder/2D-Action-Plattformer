using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirectionsCheck : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;

    CapsuleCollider2D touchingCollider;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];

    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded { get
        {
            return _isGrounded;
        }
        private set 
        { 
            _isGrounded= value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        touchingCollider= GetComponent<CapsuleCollider2D>();
        animator= GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        IsGrounded = touchingCollider.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
    }

}
