using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UIElements.Experimental;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animPlayer;
    private SpriteRenderer spritePlayer;
    private BoxCollider2D collBody;
    private BoxCollider2D collFeet;

    [SerializeField] private LayerMask jumpableTerrain;

    private enum DisplayAnimation { anim_idle, anim_running, anim_jumping, anim_falling, anim_doubleJump, anim_wallHang }

    float xAxis = 0f;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpPower = 16f;

    private bool hasDoubleJumpe;
    private bool hasWallJump;
    private bool isTouchingWall = false;
    private bool isStickingToWall = false;
    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource dashSoundEffect;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float dashSpeed = 1f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool hasWaited = true;
    private bool isDashing = false;
    private TrailRenderer trailSprite;
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        trailSprite = GetComponent<TrailRenderer>();
        collBody = GetComponent<BoxCollider2D>();
        collFeet = GetComponent<BoxCollider2D>();
        LoadPlayerData();
    }

    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if(!isDashing && (rigidbodyPlayer.bodyType != RigidbodyType2D.Static)) 
        {
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
        }
        

        if (Input.GetButtonDown("Dash") && hasWaited && (rigidbodyPlayer.bodyType != RigidbodyType2D.Static))
        {
            StartCoroutine(Dash());
        }

        UpdateAnimations();
    }

    private IEnumerator Dash()
    {

        float originalGravity = rigidbodyPlayer.gravityScale;
        rigidbodyPlayer.gravityScale = 0f;
        isDashing = true;
        hasWaited = false;

        float dashDirection = xAxis != 0f ? xAxis : (spritePlayer.flipX ? -1f : 1f);
        rigidbodyPlayer.velocity = new Vector2(dashSpeed * dashDirection, 0f);
        trailSprite.emitting = true;
        dashSoundEffect.Play();
        yield return new WaitForSeconds(dashTime);
        trailSprite.emitting = false;
        rigidbodyPlayer.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        hasWaited = true;
    }
    private void UpdateAnimations()
    {
        DisplayAnimation status;

        if (isStickingToWall && xAxis > 0f)
        {
            status = DisplayAnimation.anim_wallHang;
            spritePlayer.flipX = false;
        }
        else if (isStickingToWall && xAxis < 0f)
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
            if (hasDoubleJumpe == false)
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

    public void SavePlayerData()
    {
        SpawnPoint spawn = GetComponent<SpawnPoint>();
        int lvl = SceneManager.GetActiveScene().buildIndex;
        SaveProgress.SavePlayer(this, spawn, lvl);
    }

    public void LoadPlayerData() 
    {
        int lvl = SceneManager.GetActiveScene().buildIndex;
        SaveSystem data = null;
        data = SaveProgress.LoadData(lvl);

        if (data != null)
        {
            Vector2 positionPlayer;
            Vector2 positionSpawn;

            positionPlayer.x = data.PlayerPosition[0];
            positionPlayer.y = data.PlayerPosition[1];
            transform.position = positionPlayer;

            positionSpawn.x = data.LastSpawnpoint[0];
            positionSpawn.y = data.LastSpawnpoint[1];
            SpawnPoint.ReactivateCheckpoint(positionSpawn);
        }
    }
}   