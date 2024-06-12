using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animPlayer;
    private SpriteRenderer spritePlayer;
    private BoxCollider2D collBody;
    private BoxCollider2D collFeet;

    [SerializeField] private LayerMask jumpableTerrain;

    private enum DisplayAnimation {anim_idle, anim_running, anim_jumping, anim_falling, anim_doubleJump, anim_wallHang}

    float xAxis = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 14f;

    private bool hasDoubleJumpe;
    private bool hasWallJump;
    private bool isTouchingWall = false;
    private bool isStickingToWall = false;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private LayerMask wallLayer;
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        collBody = GetComponent<BoxCollider2D>();
        collFeet = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        rigidbodyPlayer.velocity = new Vector2(xAxis * moveSpeed, rigidbodyPlayer.velocity.y);

        StickingToAWall();

        if (Input.GetButtonDown("Jump"))
        {
            if (TouchingJumpableTerrain() || hasDoubleJumpe || hasWallJump)
            {
                jumpSoundEffect.Play();
                rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);

                if (!TouchingJumpableTerrain())
                {
                    hasDoubleJumpe = false;
                }
                else
                {
                    hasDoubleJumpe = true;
                }

                if (!isStickingToWall) 
                {
                    hasWallJump = false;
                }
                else
                {
                    hasWallJump = true;
                }    

            }
        }

        if (TouchingJumpableTerrain())
        {
            hasDoubleJumpe = true;
        }

        if (isStickingToWall)
        {
            hasWallJump = true;
        }

        UpdateAnimations();
    }


    private void UpdateAnimations()
    {
        DisplayAnimation status;

        if(isStickingToWall && xAxis > 0f)
        {
            status = DisplayAnimation.anim_wallHang;
            spritePlayer.flipX = false;
        }
        else if(isStickingToWall && xAxis < 0f)
        {
            status = DisplayAnimation.anim_wallHang;
            spritePlayer.flipX = true;
        }
        else if (xAxis > 0f)
        {
            status = DisplayAnimation.anim_running;
            spritePlayer.flipX = false;
        }
        else if (xAxis < 0f)
        {
            status = DisplayAnimation.anim_running;
            spritePlayer.flipX = true;
        }
        else
        {
            status = DisplayAnimation.anim_idle;
        }

        if (rigidbodyPlayer.velocity.y > 0.01f)
        {
            status = DisplayAnimation.anim_jumping;
            if(hasDoubleJumpe == false)
            {
                status = DisplayAnimation.anim_doubleJump;
            }
        }
        else if (rigidbodyPlayer.velocity.y < -0.01f)
        {
            status = DisplayAnimation.anim_falling;
        }

        animPlayer.SetInteger("AnimState", (int)status);
    }

    private bool TouchingJumpableTerrain()
    {
        return Physics2D.BoxCast(collFeet.bounds.center, collFeet.bounds.size, 0f, Vector2.down, 0.05f, jumpableTerrain);
    }

    private void StickingToAWall()
    {
        float wallCheckDistance = 1f;
        RaycastHit2D wallCheckLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, wallLayer);
        RaycastHit2D wallCheckRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, wallLayer);
        isTouchingWall = wallCheckLeft.collider != null || wallCheckRight.collider != null;

        if (isTouchingWall && !TouchingJumpableTerrain() && xAxis != 0)
        {
            isStickingToWall = true;
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, 0);
        }
        else
        {
            isStickingToWall = false;
        }
    }

}
