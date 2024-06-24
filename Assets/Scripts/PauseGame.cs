using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool paused;

    private PlayerMovement playerMovement;
    private GameObject player;

    private void Start()
    {
        pauseMenu.SetActive(false);
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        playerMovement.SavePlayerData();
        SceneManager.LoadScene("StartScreen");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        int lvl = SceneManager.GetActiveScene().buildIndex;
        if (lvl == 1)
        {
            SaveProgress.DeleteSaveFile1();
        }
        else if (lvl == 2) 
        {
            SaveProgress.DeleteSaveFile2();
        }
        else if ( lvl == 3)
        {
            SaveProgress.DeleteSaveFile3();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void ResumeGame()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }
}
