using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 临时写法 后续要换成entity
/// </summary>
public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;


    private float dirX = 0f;
    private float lastDirX = 0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckDistance = 0.1f;


    private enum MovementState
    {
        idle,
        running,
        jumping,
        falling
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        lastDirX = dirX;
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }


    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f && IsGrounded())
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f && IsGrounded())
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            if (lastDirX > 0f) sprite.flipX = true;
            else sprite.flipX = false;
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f,
            LayerMask.GetMask("Ground") | LayerMask.GetMask("Special"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pop"))
        {
            rb.velocity = new Vector2(0, jumpForce * 3);
            UpdateAnimationState();
        }
    }
}