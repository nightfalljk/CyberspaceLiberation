using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/AudioSettings")]
public class AudioCustomSettings : ScriptableObject
{
    public bool Music = true;
    public float MusicValue = 0.5f;
    
    public bool Sounds = true;
    public float SoundsValue = 0.5f;
}
