using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/WeaponConfig")]
public class WeaponConfig : ScriptableObject
{
    public GameObject bulletPrefab;
    public float fireRate;
    public float damage;
    public int ammo = -1;
    public int reloadTime;
    public float timeToAutomaticReload;

    public WeaponConfig(WeaponConfig weaponConfig)
    {
        this.bulletPrefab = weaponConfig.bulletPrefab;
        this.fireRate = weaponConfig.fireRate;
        this.damage = weaponConfig.damage;
        this.ammo = weaponConfig.ammo;
        this.reloadTime = weaponConfig.reloadTime;
        this.timeToAutomaticReload = weaponConfig.timeToAutomaticReload;
    }
}
