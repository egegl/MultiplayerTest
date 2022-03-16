using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
SCRIPT FOR THE SOUND OBJECTS ON THE AUDIO MANAGER PREFAB
FOLLOWED A BRACKEYS TUTORIAL FOR THIS ONE
*/

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}