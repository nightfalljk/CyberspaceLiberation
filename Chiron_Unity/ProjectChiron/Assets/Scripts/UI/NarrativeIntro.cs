using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NarrativeIntro : MonoBehaviour
{

    [SerializeField, TextArea] private String[] storyBits;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject storyPrefab;
    [SerializeField] private GameObject menuPrefab;
    [SerializeField] private GameObject videoBackground;
    
    private int current;

    private void Awake()
    {
        if (NarrativeTold.Instance.narrativeTold)
        {
            menuPrefab.SetActive(true);
            videoBackground.SetActive(true);
            storyPrefab.SetActive(false);
        }
        else
        {
            current = 0;
            textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(storyBits[current]);
        }
        
    }

    public void ContinueStory()
    {
        
        current++;
        if (current < storyBits.Length)
        {
            textBox.GetComponentInChildren<TextMeshProUGUI>().SetText(storyBits[current]);
        }
        else
        {
            //TODO: Fade to black would be nice 
            menuPrefab.SetActive(true);
            videoBackground.SetActive(true);
            storyPrefab.SetActive(false);
            NarrativeTold.Instance.narrativeTold = true;
        }
    }

}
