using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishScript : MonoBehaviour
{
    [SerializeField] private AudioSource levelFinish;
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            levelFinish.Play();
            yield return new WaitForSeconds(1);
            rb.bodyType = RigidbodyType2D.Static;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
