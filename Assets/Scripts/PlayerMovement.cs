using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animPlayer;
    private SpriteRenderer spritePlayer;
    private BoxCollider2D collBody;

    [SerializeField] private LayerMask jumpableTerrain;

    private enum DisplayAnimation {anim_idle, anim_running, anim_jumping, anim_falling}

    float xAxis = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 14f;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        collBody = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        rigidbodyPlayer.velocity = new Vector2(xAxis * moveSpeed, rigidbodyPlayer.velocity.y);

        //Jumping
        if(Input.GetButtonDown("Jump") && TouchingJumpableTerrain())
        {
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);
        }

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        DisplayAnimation status;

        if (xAxis > 0f)
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
        }
        else if (rigidbodyPlayer.velocity.y < -0.01f)
        {
            status = DisplayAnimation.anim_falling;
        }

        animPlayer.SetInteger("AnimState", (int)status);
    }

    private bool TouchingJumpableTerrain()
    {
        return Physics2D.BoxCast(collBody.bounds.center, collBody.bounds.size, 0f, Vector2.down, 0.01f, jumpableTerrain);
    }

}
