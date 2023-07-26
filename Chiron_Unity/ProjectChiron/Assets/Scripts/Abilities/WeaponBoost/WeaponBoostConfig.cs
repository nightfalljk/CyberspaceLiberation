using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/WeaponPowerConfig")]
public class WeaponBoostConfig : ScriptableObject
{
    public float weaponDamage;
    public float fireRate;
    public float reloadTime;
    public float channelTime;
    public float cooldown;
    public float duration;
    public ReactiveProperty<bool> tutCondition = new ReactiveProperty<bool>();

    public void Init(WeaponBoostConfig weaponBoostConfig)
    {
        this.weaponDamage = weaponBoostConfig.weaponDamage;
        this.fireRate = weaponBoostConfig.fireRate;
        this.reloadTime = weaponBoostConfig.reloadTime;
        this.channelTime = weaponBoostConfig.channelTime;
        this.cooldown = weaponBoostConfig.cooldown;
        this.duration = weaponBoostConfig.duration;
        this.tutCondition.Value = false;
    }

    public void ResetTutCondition()
    {
        tutCondition.Value = false;
    }
}
