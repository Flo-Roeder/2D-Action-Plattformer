using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 3f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;
    public float waypointReachedDistance = 0.1f;

    public Collider2D deathCollider;
    Animator anim;
    Rigidbody2D rb;
    Damageable damageable;

    Transform nextWaypoint;
    int waypointNum =0;

    private bool _hasTarget;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            anim.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return anim.GetBool(AnimationStrings.canMove);
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable= GetComponent<Damageable>();
    }

    private void Start()
    {
        nextWaypoint = waypoints[0];
    }

    private void OnEnable()
    {
        damageable.damageableDeath.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;

    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        if (distance<=waypointReachedDistance)
        {
            waypointNum++;
        }

        if (waypointNum>=waypoints.Count)
        {
            waypointNum = 0;
        }

        nextWaypoint = waypoints[waypointNum];
    }

    private void UpdateDirection()
    {
         Vector3 locScale = transform.localScale;
         if (transform.localScale.x>0)
         {
             if (rb.velocity.x<0)
             {
                 transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
             }
         }
         else
         {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
         }
    }

    public void OnDeath()
    {
        rb.gravityScale = .7f;
        deathCollider.enabled = true;
    }
}
