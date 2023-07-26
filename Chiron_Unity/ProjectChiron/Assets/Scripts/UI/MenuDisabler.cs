using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class MenuDisabler : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject ability_Panel; 
    private void Update()
    {
        if (!((KeyControl) Keyboard.current["escape"]).wasPressedThisFrame) return;
        if(!ability_Panel.activeSelf)
            menu.SetActive(!menu.activeSelf);
    }

    public void TriggerDisableMenu()
    {
        menu.SetActive(false);
    }
}
