using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animPlayer;
    private SpriteRenderer spritePlayer;

    float xAxis = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 14f;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        rigidbodyPlayer.velocity = new Vector2(xAxis * moveSpeed, rigidbodyPlayer.velocity.y);

        //Jumping
        if(Input.GetButtonDown("Jump"))
        {
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpPower);
        }

        UpdateAnimations();
    }

    private void UpdateAnimations()
    {
        if (xAxis > 0f)
        {
            animPlayer.SetBool("IsRunning", true);
            spritePlayer.flipX = false;
        }
        else if (xAxis < 0f)
        {
            animPlayer.SetBool("IsRunning", true);
            spritePlayer.flipX = true;
        }
        else
        {
            animPlayer.SetBool("IsRunning", false);
        }
    }

}
