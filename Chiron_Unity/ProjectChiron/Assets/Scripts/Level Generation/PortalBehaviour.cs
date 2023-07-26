using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level_Generation
{
    public class PortalBehaviour : MonoBehaviour
    {
        private LevelGenerator _levelGenerator;
        private PlayerCharacterController pcc; 
        private void Awake()
        {
            _levelGenerator = FindObjectOfType<LevelGenerator>();
            pcc = FindObjectOfType<PlayerCharacterController>(); 
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name != "Tutorial")
            {
                CustomEvent.Trigger(_levelGenerator.gameObject, "DoorEnter");
            }
            else if (other.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name == "Tutorial")
            { 
                pcc.ResetLevel();
                SceneManager.LoadScene(0);   
            }
        }
    }
}
