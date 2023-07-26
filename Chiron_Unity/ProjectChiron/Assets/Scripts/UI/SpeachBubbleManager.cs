using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeachBubbleManager : MonoBehaviour
{
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            this.GameObject().SetActive(false);
        }
    }
}
