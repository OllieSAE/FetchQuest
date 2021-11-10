using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        print("Quit!");
        Application.Quit();
    }

    public void PlaySound()
    {
        FindObjectOfType<AudioManager>().Play("Bark");
    }
}
