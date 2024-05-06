using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpImpulse = 10f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;

    public float CurrentMoveSpeed
    {
        get
        {
            if (!CanMove || !IsMoving || touchingDirections.IsOnWall) return 0;
            if (!IsRunning || !touchingDirections.IsGrounded) return walkSpeed;
            return runSpeed;
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning; }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.IsRunning, value);
        }
    }

    private bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    public bool CanMove { get { return animator.GetBool(AnimationStrings.CanMove); } }
    public bool IsAlive { get { return animator.GetBool(AnimationStrings.IsAlive); } }

    Rigidbody2D rb;
    Animator animator;
    public float reloadDelay = 3.0f; // Adjust delay in seconds
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.YVelocity, rb.velocity.y);
        if (!IsAlive)
        {
            StartCoroutine(ReloadSceneWithDelay());
        }
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            setFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void setFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void onRun(InputAction.CallbackContext context)
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

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.Jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }


    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.Attack);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (damageable.isInvicible)
        {
            return;
        }
        if (collision.gameObject.layer == 10)
        {
            Debug.Log("Do something here");
            damageable.Hit(50);
        }
    }

    private IEnumerator ReloadSceneWithDelay()
    {
        yield return new WaitForSeconds(reloadDelay); // Wait for specified delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload scene
    }

}
