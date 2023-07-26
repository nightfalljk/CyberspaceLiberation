using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] private DifficultyHolder difficultyHolder;
    [SerializeField] private Toggle easy;
    [SerializeField] private Toggle hard;

    private bool easyLast;
    private bool hardLast;

    private void Awake()
    {
        easy.isOn = difficultyHolder.selectedDifficulty == 0;
        hard.isOn = difficultyHolder.selectedDifficulty == 1;

        easyLast = easy.isOn;
        hardLast = hard.isOn;
    }

    private void Update()
    {
        if (easy.isOn != easyLast)
        {
            if (easy.isOn)
            {
                difficultyHolder.selectedDifficulty = 0;
                hard.isOn = false;
                hardLast = false;
            }
            else
            {
                easy.isOn = true;
            }
            
            easyLast = easy.isOn;
        }
        
        if (hard.isOn != hardLast)
        {
            if (hard.isOn)
            {
                difficultyHolder.selectedDifficulty = 1;
                easy.isOn = false;
                easyLast = false;
            }
            else
            {
                hard.isOn = true;
            }
            
            hardLast = hard.isOn;
        }
    }
}
