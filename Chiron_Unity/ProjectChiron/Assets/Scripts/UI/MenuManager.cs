using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameObject _optionsMenu;

    public void TriggerStartGame()
    {
        SceneManager.LoadScene(1); 
    }
    public void TriggerStartCredits()
    {
        SceneManager.LoadScene(3); 
    }
    public void TriggerStartTutorial()
    {
        SceneManager.LoadScene(2); 
    }
    public void TriggerQuitGame()
    {
        Application.Quit();
    }

    public void TriggerReturnToMainMenu()
    {
//        if (FindObjectOfType<PlayerCharacterController>())
//        {
//            FindObjectOfType<PlayerCharacterController>().ResetLevel(); 
//        }
        SceneManager.LoadScene(0); 
    }

    public void ResumeGame()
    {
        this.gameObject.SetActive(false); 
    }

    public void TriggerOpenOptions(GameObject optionsMenu)
    {
        if(optionsMenu.activeSelf)
            optionsMenu.SetActive(false);
        else if(!optionsMenu.activeSelf) 
            optionsMenu.SetActive(true);
    }

    private void Update()
    {
        if (!((KeyControl) Keyboard.current["Escape"]).wasPressedThisFrame) return;
        Debug.Log("Button pressed");
        if (_optionsMenu == null) return; 
        Debug.Log("options != null");
        
        if(_optionsMenu.activeSelf)
            _optionsMenu.SetActive(false);
        else if(!_optionsMenu.activeSelf) 
            _optionsMenu.SetActive(true);
    }
}
