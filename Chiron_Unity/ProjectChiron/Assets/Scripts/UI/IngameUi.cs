using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.PlayerLoop;

public class IngameUi : MonoBehaviour
{
    [SerializeField] private AbilityReferenceHolder dash;
    [SerializeField] private AbilityReferenceHolder teleport;
    [SerializeField] private AbilityReferenceHolder secondLife;
    [SerializeField] private AbilityReferenceHolder weaponBoost;
    [SerializeField] private AbilityReferenceHolder slowField;
    [SerializeField] private AbilityReferenceHolder hack;

    private PlayerCharacterController _pcc;
    
    //quickfix for tutorial
    private void Update()
    {
        if (_pcc == null)
            return;
        
        dash.AbilityOn.SetActive(_pcc.DashEnabled.Value && _pcc.DashAvailable.Value);
        dash.AbilityOff.SetActive(_pcc.DashEnabled.Value && !_pcc.DashAvailable.Value);
        if(_pcc.DashEnabled.Value)
            dash.Cooldown.value = _pcc.DashDurationPercentage.Value != 1 ? 1-_pcc.DashDurationPercentage.Value : _pcc.DashCooldownPercentage.Value;//_pcc.DashCooldownPercentage.Value +  (1 -_pcc.DashDurationPercentage.Value);
        
        teleport.AbilityOn.SetActive(_pcc.TeleportEnabled.Value && _pcc.TeleportAvailable.Value);
        teleport.AbilityOff.SetActive(_pcc.TeleportEnabled.Value && !_pcc.TeleportAvailable.Value);
        if(_pcc.TeleportEnabled.Value)
            teleport.Cooldown.value = _pcc.TeleportCooldownPercentage.Value;// +  (_pcc.TeleportDurationPercentage.Value);
        
        secondLife.AbilityOn.SetActive(_pcc.SecondLifeEnabled.Value && _pcc.SecondLifeAvailable.Value);
        secondLife.AbilityOff.SetActive(_pcc.SecondLifeEnabled.Value && !_pcc.SecondLifeAvailable.Value);
        if(_pcc.SecondLifeEnabled.Value)
            secondLife.Cooldown.value = _pcc.SecondLifeAvailable.Value ? 1 : 0;
        
        weaponBoost.AbilityOn.SetActive(_pcc.WeaponBoostEnabled.Value && _pcc.WeaponBoostAvailable.Value);
        weaponBoost.AbilityOff.SetActive(_pcc.WeaponBoostEnabled.Value && !_pcc.WeaponBoostAvailable.Value);
        if(_pcc.WeaponBoostEnabled.Value)
            weaponBoost.Cooldown.value = _pcc.WeaponBoostDuration.Value != 1 ? 1-_pcc.WeaponBoostDuration.Value : _pcc.WeaponBoostCooldown.Value;
        
        slowField.AbilityOn.SetActive(_pcc.SlowFieldEnabled.Value && _pcc.SlowFieldAvailable.Value);
        slowField.AbilityOff.SetActive(_pcc.SlowFieldEnabled.Value && !_pcc.SlowFieldAvailable.Value);
        if(_pcc.SlowFieldEnabled.Value)
            slowField.Cooldown.value = _pcc.SlowFieldCooldown.Value;// + (1 - _pcc.WeaponBoostDuration.Value);
        
        hack.AbilityOn.SetActive(_pcc.HackEnabled.Value && _pcc.HackAvailable.Value);
        hack.AbilityOff.SetActive(_pcc.HackEnabled.Value && !_pcc.HackAvailable.Value);
        if(_pcc.HackEnabled.Value)
            hack.Cooldown.value = _pcc.HackDurationPercentage.Value != 1 ? 1-_pcc.HackDurationPercentage.Value : _pcc.HackCooldownPercentage.Value;//_pcc.HackCooldownPercentage.Value + (_pcc.HackDurationPercentage.Value);

    }

    public void SelfSet(PlayerCharacterController pcc)
    {
        _pcc = pcc;
        return;
        
        
//        pcc.DashEnabled.Where(e => e).Subscribe(b =>
//        {
//            dash.AbilityOn.SetActive(b & );
//        });
        
        pcc.DashAvailable.Where(_ => pcc.DashEnabled.Value).Subscribe(b =>
        {
            dash.AbilityOn.SetActive(b);
            dash.AbilityOff.SetActive(!b);
        });
        pcc.DashAvailable.Where(_ => !pcc.DashEnabled.Value).Subscribe(b =>
        {
            dash.AbilityOn.SetActive(false);
            dash.AbilityOff.SetActive(false);
        });
        
        pcc.TeleportAvailable.Where(_ => pcc.TeleportEnabled.Value).Subscribe(b =>
        {
            teleport.AbilityOn.SetActive(b);
            teleport.AbilityOff.SetActive(!b);
        });
        pcc.TeleportAvailable.Where(_ => !pcc.TeleportEnabled.Value).Subscribe(b =>
        {
            teleport.AbilityOn.SetActive(false);
            teleport.AbilityOff.SetActive(false);
        });
        
        pcc.SecondLifeAvailable.Where(_ => pcc.SecondLifeEnabled.Value).Subscribe(b =>
        {
            secondLife.AbilityOn.SetActive(b);
            secondLife.AbilityOff.SetActive(!b);
        });
        pcc.SecondLifeAvailable.Where(_ => !pcc.SecondLifeEnabled.Value).Subscribe(b =>
        {
            secondLife.AbilityOn.SetActive(false);
            secondLife.AbilityOff.SetActive(false);
        });
        
        pcc.WeaponBoostAvailable.Where(_ => pcc.WeaponBoostEnabled.Value).Subscribe(b =>
        {
            weaponBoost.AbilityOn.SetActive(b);
            weaponBoost.AbilityOff.SetActive(!b);
        });
        pcc.WeaponBoostAvailable.Where(_ => !pcc.WeaponBoostEnabled.Value).Subscribe(b =>
        {
            weaponBoost.AbilityOn.SetActive(false);
            weaponBoost.AbilityOff.SetActive(false);
        });
        
        pcc.SlowFieldAvailable.Where(_ => pcc.SlowFieldEnabled.Value).Subscribe(b =>
        {
            slowField.AbilityOn.SetActive(b);
            slowField.AbilityOff.SetActive(!b);
        });
        pcc.SlowFieldAvailable.Where(_ => !pcc.SlowFieldEnabled.Value).Subscribe(b =>
        {
            slowField.AbilityOn.SetActive(false);
            slowField.AbilityOff.SetActive(false);
        });
        
        pcc.HackAvailable.Where(_ => pcc.HackEnabled.Value).Subscribe(b =>
        {
            hack.AbilityOn.SetActive(b);
            hack.AbilityOff.SetActive(!b);
        });
        pcc.HackAvailable.Where(_ => !pcc.HackEnabled.Value).Subscribe(b =>
        {
            hack.AbilityOn.SetActive(false);
            hack.AbilityOff.SetActive(false);
        });
    }
}

