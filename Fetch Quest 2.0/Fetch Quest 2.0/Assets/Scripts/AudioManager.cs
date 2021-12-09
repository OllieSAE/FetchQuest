using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private Scene currentScene;

    void Awake()
    {
        //if there is an existing AudioManager instance, then any new ones (created on loading new scene), are destroyed
        //there can only be one Highla- AudioManager!
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            
            s.source.outputAudioMixerGroup = s.group;
        }

        Play("MainMenuMusic");
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        
        //important to have the second check, otherwise sound file will repeatedly start and it sounds like a jarring hum
        
        if ((sceneName != "Main Menu") && (!IsPlaying("Background")) && (sceneName != "End Menu"))
        {
            //StartCoroutine(FadeOut("MainMenuMusic", 0.2f));
            if ((sceneName == "Level1") || (sceneName == "Level2") || (sceneName == "Level3"))
            {
                Stop("MainMenuMusic");
                Stop("ATumbleDown");
                Play("Background");
            }
            //else
            //{
                //Stop("MainMenuMusic");
                //Play("ATumbleDown");
            //}
        }

        if ((sceneName == "Main Menu") && (!IsPlaying("MainMenuMusic")))
        {
            Stop("Background");
            Stop("ATumbleDown");
            Play("MainMenuMusic");
        }

        if ((sceneName == "End Menu") && (!IsPlaying("MainMenuMusic")))
        {
            Stop("Background");
            Stop("ATumbleDown");
            Stop("Running");
            Play("MainMenuMusic");
        }

        if (((sceneName == "Level4") || (sceneName == "Level5")) && (IsPlaying("Background")))
        {
            Stop("Background");
            Play("ATumbleDown");
        }
    }

    //public IEnumerator FadeOut(string name, float FadeTime)
    //{
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        //if (s == null)
        //{
            //print("Sound " + name + " was not found!");
        //}
        //float startVolume = s.volume;

        //while (s.volume > 0)
        //{
            //s.volume -= startVolume * Time.deltaTime / FadeTime;
            //yield return null;
        //}

        //Stop("MainMenuMusic");
        //s.volume = startVolume;
    //}
    public void Play(string name)
    {
        //searches the "sounds Array", for a sound that is equal to the "name" parameter sent in
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name +" was not found!");
            return;
        }
        s.source.Play();
        
        //TO PLAY - Find where you want the audio to be called from...
        
        //FindObjectOfType<AudioManager>().Play("string name goes here")
    }

    public void PlayOneShot(AudioClip clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name +" was not found!");
            return;
        }
        s.source.PlayOneShot(clip, 1f);
    }
    
    public void Stop(string name)
    {
        //searches the "sounds Array", for a sound that is equal to the "name" parameter sent in
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name + " was not found!");
            return;
        }
        s.source.Stop();
        
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + name + " was not found!");
            return false;
        }
        return s.source.isPlaying;
    }
}
