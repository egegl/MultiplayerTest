using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
AUDIO MANAGER, DROP ALL THE SOUND SOURCES ON THE PREFAB THIS SCRIPT'S ON
FOLLOWED A BRACKEYS TUTORIAL FOR THIS ONE, THEN ADDED A STOP() METHOD TO STOP SOUNDS
*/

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
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
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (Time.timeScale == 0) // PAUSE AUDIO IF TIME SCALE IS 0 (GAME IS PAUSED) 
        {
            s.source.pitch = 0;
        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
}
