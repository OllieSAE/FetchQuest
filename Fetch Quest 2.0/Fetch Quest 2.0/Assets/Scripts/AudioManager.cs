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
        }

        Play("MainMenuMusic");
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        
        if ((sceneName != "Main Menu") && (!IsPlaying("Background")))
        {
            Stop("MainMenuMusic");
            Play("Background");
        }

        if ((sceneName == "Main Menu") && (!IsPlaying("MainMenuMusic")))
        {
            Stop("Background");
            Play("MainMenuMusic");
        }
    }

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
