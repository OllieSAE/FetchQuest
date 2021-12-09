using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //public Animator playAnimator;
    //public Animator controlsAnimator;
    //public Animator settingsAnimator;
    //public Animator creditsAnimator;
    //public Animator exitAnimator;
    //private bool hasPlayed = false;

    private void Start()
    {
        //animator = this.GetComponent(animator)
    }

    //private void Update()
    //{
        //if (playAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")||controlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")||settingsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")||creditsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")||exitAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted"))
        //{
            //if ((hasPlayed == false)&&(!FindObjectOfType<AudioManager>().IsPlaying("ButtonHighlight")))
            //{
                //FindObjectOfType<AudioManager>().Play("ButtonHighlight");
                
            //}
            //hasPlayed = true;
        //}
        
        //if(!playAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")&&!controlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")&&!settingsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")&&!creditsAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted")&&!exitAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted"))
        //{
            //hasPlayed = false;
        //}
    //}

    public void PlayHighlightButton()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHighlight");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
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
