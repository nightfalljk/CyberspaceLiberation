using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AbilitySelect : MonoBehaviour
{
    [SerializeField] private Button ContinueButton;
    
    [SerializeField] private Button DashButton;
    [SerializeField] private Button TeleportButton;
    [SerializeField] private Button SecondLifeButton;
    [SerializeField] private Button WeaponBoostButton;
    [SerializeField] private Button SlowFieldButton;
    [SerializeField] private Button HackButton;
    //[HideInInspector] public PlayerCharacterController pcc;

    [SerializeField] private AbilityReferenceHolder dash;
    [SerializeField] private AbilityReferenceHolder teleport;
    [SerializeField] private AbilityReferenceHolder secondLife;
    [SerializeField] private AbilityReferenceHolder weaponBoost;
    [SerializeField] private AbilityReferenceHolder slowField;
    [SerializeField] private AbilityReferenceHolder hack;

    public void SelfSet(PlayerCharacterController pcc, UIManager uim)
    {
        ContinueButton.onClick.AddListener(uim.ExitAbilityMenu);
        
        DashButton.onClick.AddListener(pcc.EnableDash);
        TeleportButton.onClick.AddListener(pcc.EnableTeleport);
        SecondLifeButton.onClick.AddListener(pcc.EnableSecondLife);
        WeaponBoostButton.onClick.AddListener(pcc.EnableWeaponBoost);
        SlowFieldButton.onClick.AddListener(pcc.EnableSlowField);
        HackButton.onClick.AddListener(pcc.EnableHack);
        
        pcc.DashEnabled.Subscribe(b =>
        {
            dash.AbilityOn.SetActive(b);
            dash.AbilityOff.SetActive(!b);
        });
        pcc.TeleportEnabled.Subscribe(b =>
        {
            teleport.AbilityOn.SetActive(b);
            teleport.AbilityOff.SetActive(!b);
        });
        pcc.SecondLifeEnabled.Subscribe(b =>
        {
            secondLife.AbilityOn.SetActive(b);
            secondLife.AbilityOff.SetActive(!b);
        });
        pcc.WeaponBoostEnabled.Subscribe(b =>
        {
            weaponBoost.AbilityOn.SetActive(b);
            weaponBoost.AbilityOff.SetActive(!b);
        });
        pcc.SlowFieldEnabled.Subscribe(b =>
        {
            slowField.AbilityOn.SetActive(b);
            slowField.AbilityOff.SetActive(!b);
        });
        pcc.HackEnabled.Subscribe(b =>
        {
            hack.AbilityOn.SetActive(b);
            hack.AbilityOff.SetActive(!b);
        });
    }
}
