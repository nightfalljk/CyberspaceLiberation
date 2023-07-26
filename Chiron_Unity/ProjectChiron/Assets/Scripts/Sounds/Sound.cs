using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

[System.Serializable]
public class Sound{

    public string name;

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume = 0.5f;
    [Range(.1f,3f)]
    public float pitch = 1f;

    public bool loop;
//    [HideInInspector]
//    public bool playOnStart = false; // Use AudioStarter Instead
    public bool isMusic = false;
    public bool singleton = false;
    public float startOffset = 0;
    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public float fadeVolume = 1;

    //[HideInInspector] public bool isPlaying;
}
