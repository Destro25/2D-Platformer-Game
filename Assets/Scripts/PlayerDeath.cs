using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private Rigidbody2D rigidbodyPlayer;
    private Animator animPlayer;
    private BoxCollider2D collPlayer;
    // Start is called before the first frame update
    [SerializeField] private float DeathFloor = -14.0f;
    [SerializeField] private AudioSource deathSoundEffect;
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        collPlayer = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    private void Update() 
    { 
        if (DeathFloor > transform.position.y && collPlayer.enabled == true)
        {
            Die();
        }
    }

    private void Die()
    {
        deathSoundEffect.Play();
        rigidbodyPlayer.bodyType = RigidbodyType2D.Static;
        collPlayer.enabled = false;
        animPlayer.SetTrigger("DeathTrigger");
    }

    private void Restart()
    {
        rigidbodyPlayer.bodyType = RigidbodyType2D.Dynamic;
        SpawnPoint.Spawn();
        collPlayer.enabled = true;
    }
}
