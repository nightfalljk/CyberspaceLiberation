using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class WeaponBoost : MonoBehaviour
{

    private WeaponBoostConfig _weaponBoostConfig;
    private ProjectileLauncher _projectileLauncher;
    private ReactiveProperty<bool> _boostingWeaponAvailable;
    private ReactiveProperty<bool> _weaponBoostEnabled;
    private ReactiveProperty<float> _weaponBoostCooldown;
    private ReactiveProperty<float> _weaponBoostDuration;
    

    public void SetWeaponBoostConfig(WeaponBoostConfig weaponBoostConfig)
    {
        this._weaponBoostConfig = weaponBoostConfig;
    }

    public void SetProjectileLaucher(ProjectileLauncher projectileLauncher)
    {
        this._projectileLauncher = projectileLauncher;
    }
    
    private void Awake()
    {
        _boostingWeaponAvailable = new ReactiveProperty<bool>();
        _weaponBoostEnabled = new ReactiveProperty<bool>();
        _weaponBoostCooldown = new ReactiveProperty<float>();
        _weaponBoostDuration = new ReactiveProperty<float>();
        _weaponBoostCooldown.Value = 1;
        _weaponBoostDuration.Value = 1;
        _boostingWeaponAvailable.Value = true;
        _weaponBoostEnabled.Value = false;
    }
    
    public IEnumerator BoostingWeapon()
    {
        if (!_weaponBoostConfig.tutCondition.Value)
            _weaponBoostConfig.tutCondition.Value = true;
        
        _boostingWeaponAvailable.Value = false;
        yield return new WaitForSeconds(_weaponBoostConfig.channelTime);
        _projectileLauncher.SetDamage(_weaponBoostConfig.weaponDamage);
        _projectileLauncher.SetFireRate(_weaponBoostConfig.fireRate);
        _projectileLauncher.SetReloadTime(_weaponBoostConfig.reloadTime);
        yield return new WaitForDuration(_weaponBoostConfig.duration, _weaponBoostDuration);
        _projectileLauncher.ResetAfterAbility();
        yield return new WaitForCooldown(_weaponBoostConfig.cooldown, _weaponBoostCooldown);
        _boostingWeaponAvailable.Value = true;
    }

    public void ResetOnNewLevel()
    {
        _boostingWeaponAvailable.Value = true;
        _weaponBoostCooldown.Value = 1;
        _weaponBoostDuration.Value = 1;
    }

    public ReactiveProperty<bool> BoostingWeaponAvailable => _boostingWeaponAvailable;

    public float ChannelingTime => _weaponBoostConfig.channelTime;

    public ReactiveProperty<bool> WeaponBoostEnabled
    {
        get => _weaponBoostEnabled;
        set => _weaponBoostEnabled = value;
    }

    public ReactiveProperty<float> WeaponBoostCooldown => _weaponBoostCooldown;

    public ReactiveProperty<float> WeaponBoostDuration => _weaponBoostDuration;
}
