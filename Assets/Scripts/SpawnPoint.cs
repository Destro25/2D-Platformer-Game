using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static Vector2 spawn;
    private static Rigidbody2D rigidbodyPlayer;
    private static Animator animCheckpoint = null;
    [SerializeField] private AudioSource checkpointTrigger;
    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        spawn = rigidbodyPlayer.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Checkpoint"))
        {
            animCheckpoint = collision.GetComponent<Animator>();
            if(animCheckpoint.GetBool("isActive") == false)
            {
                checkpointTrigger.Play();
                spawn = collision.transform.position;
                animCheckpoint.SetBool("isActive", true);
            }
        }
    }

    public static void Spawn()
    {
        rigidbodyPlayer.transform.position = spawn;
    }
}
