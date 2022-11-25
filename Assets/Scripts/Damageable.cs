using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageHit;
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
            if (Health<=0)
            {
                IsAlive = false;
            }
        }
    }

    private bool _isAlive = true;
    [SerializeField] private bool isInvincible;

    public bool IsHit
    {
        get
        {
            return anim.GetBool(AnimationStrings.isHit);
        }
        private set
        {
            anim.SetBool(AnimationStrings.isHit, value);
        }
    }

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
    }

    public void Hit(int damage, Vector2 knockback) 
    {
        if (IsAlive
            && !isInvincible)
        {
            Health -= damage;
            isInvincible= true;

            IsHit = true;
            damageHit.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
        }
    }

    public void Heal(int healAmount)
    {
        if (IsAlive) 
        {
            int maxHeal= Mathf.Max(MaxHealth-Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healAmount);
            Health += actualHeal;
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
        }
    }
}
