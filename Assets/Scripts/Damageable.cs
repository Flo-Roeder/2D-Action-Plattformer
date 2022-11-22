using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator anim;

    [SerializeField] private int _maxHealth =100 ;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField] private int _health = 100;
    public int Health
    {
        get 
        {
            return _health; 
        }
        set
        {
            _health = value;
            if (Health<0)
            {
                IsAlive = false;
            }
        }
    }

    private bool _isAlive = true;
    [SerializeField] private bool isInvincible;
    private float timeSinceHit=0;
    public float invincibleTime = 0.25f;

    public bool IsAlive {
        get
        {
            return _isAlive;
        }
        private set 
        {
            _isAlive = value;
            anim.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("Is alive set" + value);
        } 
    }

    void Awake()
    {
        anim= GetComponent<Animator>();
    }

    void Update()
    {
        if (isInvincible) 
        {
            if (timeSinceHit > invincibleTime)
            {
                isInvincible= false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
        Hit(10);
    }

    public void Hit(int damage) 
    {
        if (IsAlive
            && !isInvincible)
        {
            Health -= damage;
            isInvincible= true;
        }
    }
}
