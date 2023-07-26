using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuManager : MonoBehaviour
{
    private GameObject optionsMenu;
    
    [SerializeField] private AudioCustomSettings audioCustomSettings;
    [SerializeField] private Slider musicSlider; 
    [SerializeField] private Slider sfxSlider; 
    [SerializeField] private Toggle musicMute; 
    [SerializeField] private Toggle sfxMute; 

    private void Awake()
    {
        optionsMenu = this.GameObject();
    }

    private void OnEnable()
    {
        Debug.Log("Enabled");
        musicMute.isOn = audioCustomSettings.Music;
        sfxMute.isOn = audioCustomSettings.Sounds;
        musicSlider.value = audioCustomSettings.MusicValue;
        sfxSlider.value = audioCustomSettings.SoundsValue;
    }

    public void MusicVolumeControl(Slider slider)
    {
        audioCustomSettings.MusicValue = slider.value; 
    }

    public void SoundEffectControl(Slider slider)
    {
        audioCustomSettings.SoundsValue = slider.value; 
    }

    public void OpenMenu()
    {
        optionsMenu.SetActive(true);
    }
    
    public void CloseMenu()
    {
        optionsMenu.SetActive(false);
    }

    public void MuteMusicVolume(Toggle muteToggle)
    {
        audioCustomSettings.Music = muteToggle.isOn;
    }
    
    public void MuteSoundEffectVolume(Toggle muteToggle)
    {
        audioCustomSettings.Sounds = muteToggle.isOn;
    }
}
