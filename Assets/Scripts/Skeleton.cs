using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public float walkSpeed = 4f;
    Rigidbody2D rb;
    Animator animator;
    public enum WalkableDirection { Right, Left }
    TouchingDirections touchingDirections;
    Damageable damageable;

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.left;
    public bool IsAlive { get { return animator.GetBool(AnimationStrings.IsAlive); } }
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            if (value == WalkableDirection.Right)
            {
                walkDirectionVector = Vector2.right;
            }
            else if (value == WalkableDirection.Left)
            {
                walkDirectionVector = Vector2.left;
            }

            _walkDirection = value;
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.CanMove); }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if (CanMove)
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            damageable.Hit(20);
            Debug.Log("Esqueleto tomou dano");
        }
    }

}
