using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
        
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
        Play("Background");
    }

    public void Play(string name)
    {
        //searches the "sounds Array", for a sound that is equal to the "name" parameter sent in
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            print("Sound " + " was not found!");
            return;
        }
        s.source.Play();
        
        //TO PLAY - Find where you want the audio to be called from...
        
        //FindObjectOfType<AudioManager>().Play("string name goes here")
    }
}
