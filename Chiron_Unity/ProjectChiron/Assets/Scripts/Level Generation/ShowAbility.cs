using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ShowAbility : MonoBehaviour
{
    [SerializeField] private PlayerCharacterController pcc;
    [SerializeField] private GameObject speachBubble;

    private TextMeshProUGUI tmp;
    private TMP_SpriteAsset tmp_sa; 

    private Dictionary<string, string> texts = new Dictionary<string, string>(); 
    
    private void Awake()
    {
        tmp = speachBubble.GetComponentInChildren<TextMeshProUGUI>();
        tmp_sa = speachBubble.GetComponentInChildren<TextMeshProUGUI>().spriteAsset; 

        texts.Add("Teleport", "Teleport: Rematerialise somewhere in Cyberspace for a\nstrategic flanking maneuver.\n Activate with Space, confirm with LMB, abort with RMB."); 
        texts.Add("Dash", "Dash: Hypercharge your Cyberboard to find trouble faster.\nActivate with Space. What else would you use it for?"); 
        texts.Add("SlowField", "Time Grenade:  With it's temporal distortion field this grenade\n Activate with RMB, slows down all enemies and projectiles in it's area."); 
        texts.Add("SecondLife", "Second Life: On death you simply... Don't die.\nIncredible technology.\n Disclaimer: Only when selected and in Cyberspace.");  
        texts.Add("Hack", "Hack: Take over an enemy and have it rain\nActivate with RMB, death and destruction on it's friends and family.\nFor a while..."); 
        texts.Add("WeaponBoost", "Overload: Overload your power glove to deal massive\nActivate with Shift, damage at incredible speeds.\nUnlimited. Power."); 
        texts.Add("Controls", "Use" + " <sprite=0> <sprite=1> <sprite=2> <sprite=3>" + " to move.");
        texts.Add("Shoot", "Press <sprite name=mouse-left-button>to fire your power glove.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;  
        speachBubble.SetActive(true) ; 
        switch (this.gameObject.name)
        {
            case "Teleport":
                pcc.EnableTeleport();
                tmp.text = texts["Teleport"]; 
                pcc.TutEnableTeleport();
                break;
            case "Dash":
                pcc.EnableDash();
                tmp.text = texts["Dash"]; 
                pcc.TutEnableDash();
                break;
            case "SlowField":
                pcc.EnableSlowField();
                tmp.text = texts["SlowField"]; 
                pcc.TutEnableSlowField();
                break;
            case "SecondLife":
                pcc.EnableSecondLife();
                tmp.text = texts["SecondLife"]; 
                pcc.TutEnableSecondLife();
                break;
            case "Hack":
                pcc.EnableHack();
                tmp.text = texts["Hack"]; 
                pcc.TutEnableHack();
                break;
            case "WeaponBoost":
                pcc.EnableWeaponBoost();
                tmp.text = texts["WeaponBoost"]; 
                pcc.TutEnableWeaponBoost();
                break;
            case "Controls":
                tmp.text = texts["Controls"]; 
                break;
            case "Shoot":
                tmp.text = texts["Shoot"];
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        speachBubble.SetActive(false);
    }
}
