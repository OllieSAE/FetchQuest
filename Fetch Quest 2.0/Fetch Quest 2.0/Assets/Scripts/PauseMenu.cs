using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Levels/Main Menu");
    }

    public void QuitGame()
    {
        print("Quitting game...");
        Application.Quit();
    }
    
    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("Bark");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Levels/Level1");
        Resume();
    }
    
    public void LoadLevel2()
    {
        SceneManager.LoadScene("Levels/Level2");
        Resume();
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Levels/Level3");
        Resume();
    }

    public void LoadLevel4()
    {
        SceneManager.LoadScene("Levels/Level4");
        Resume();
    }

    public void LoadLevel5()
    {
        SceneManager.LoadScene("Levels/Level5");
        Resume();
    }

}
