using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioStarter : MonoBehaviour
{
    [Header("Start of Scene")]
    public bool stopAllMusicOnStart = false;
    public bool stopAllSoundsOnStart = false;
    public List<string> AudioToStopOnStart;
    public List<string> AudioToStartOnStart;
    
    [Header("End of Scene")]
    public bool stopAllMusicOnEnd = false;
    public bool stopAllSoundsOnEnd = false;

    public List<AudioHelper> excludeOnEnd;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.StopAllSounds(stopAllMusicOnStart, stopAllSoundsOnStart);
        foreach (string s in AudioToStopOnStart)
        {
            AudioManager.Instance.Stop(s, true);
        }
        
        foreach (string s in AudioToStartOnStart)
        {
            AudioManager.Instance.Play(s, smooth:true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds(stopAllMusicOnEnd, stopAllSoundsOnEnd, excludeOnEnd);
    }
}
