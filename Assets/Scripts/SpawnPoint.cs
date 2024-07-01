using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private static Vector2 spawn;
    private static Rigidbody2D rigidbodyPlayer;
    private static Animator checkpointAnim = null;
    [SerializeField] private AudioSource checkpointTrigger;
    
    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
        spawn = rigidbodyPlayer.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Checkpoint"))
        {
            checkpointAnim = collision.GetComponent<Animator>();
            if(checkpointAnim.GetInteger("CheckpointState") == 0)
            {
                checkpointTrigger.Play();
                ResetOtherCheckpoints();
                checkpointAnim.SetInteger("CheckpointState", 1);
                spawn = collision.transform.position;
            }
        }
    }

    public static void Spawn()
    {
        rigidbodyPlayer.transform.position = spawn;
    }

    private void ResetOtherCheckpoints()
    {
        GameObject[] otherCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach (GameObject otherCheckpoint in otherCheckpoints)
        {
            if(otherCheckpoint == gameObject)
            {
                continue;
            }
            otherCheckpoint.GetComponent<Animator>().SetInteger("CheckpointState", 0);
        }

    }

    public Vector2 getCheckpoint()
    {
        return spawn;
    }

    public static void ReactivateCheckpoint(Vector2 position, float radius = 0.5f)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        if (colliders.Length > 0)
        {
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Checkpoint"))
                {
                    checkpointAnim = collider.GetComponent<Animator>();
                    if (checkpointAnim.GetInteger("CheckpointState") == 0)
                    {
                        checkpointAnim.SetInteger("CheckpointState", 1);
                        spawn = collider.transform.position;
                    }
                }
            }
        }
    }
}
