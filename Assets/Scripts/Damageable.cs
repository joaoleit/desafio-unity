using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    public bool isInvicible = false;

    public float invicibiltyTime = 1f;
    private float timeSinceHit;

    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.IsAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit(int damage)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            animator.SetTrigger(AnimationStrings.Hurt);
        }

        isInvicible = true;
    }

    void Update()
    {
        if (isInvicible)
        {
            if (timeSinceHit > invicibiltyTime)
            {
                isInvicible = false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
    }
}
