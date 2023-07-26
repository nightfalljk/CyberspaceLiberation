using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{

    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] public Transform bulletSpawn;
    private Queue<Projectile> _projectilePool;
    private int _ammo;
    private GameObject _projectile;
    private float _fireRate;
    private float _reloadTime;
    private float _dmg;
    private int _theme;
    private bool _hacked;

    private float _timeToShoot;
    private float _timeToAutomaticReload = 0;

    private void Awake()
    {
        _projectilePool = new Queue<Projectile>();
        _timeToShoot = 0;
        _ammo = weaponConfig.ammo;
        _dmg = weaponConfig.damage;
        _fireRate = weaponConfig.fireRate;
        _reloadTime = weaponConfig.reloadTime;
        _projectile = weaponConfig.bulletPrefab;
        _hacked = false;
    }

    private void Update()
    {
        if (_timeToShoot > 0)
        {
            _timeToShoot -= Time.deltaTime;
        }
        else if (_timeToShoot < 0)
        {
            _timeToShoot = 0;
        }

        if(_timeToAutomaticReload > 0)
            _timeToAutomaticReload -= Time.deltaTime;

        if (_ammo != 0 && _ammo != weaponConfig.ammo && _timeToAutomaticReload < 0)
        {
            _timeToAutomaticReload = 0;
            StartCoroutine(Reload());
        }
    }

    private Projectile GetProjectile(Quaternion shootDir)
    {
        if (_projectilePool.Count > 0 && !_hacked)
        {
            var bullet = _projectilePool.Dequeue();
            var bulletObject = bullet.gameObject;
            bulletObject.SetActive(true);
            bulletObject.transform.position = bulletSpawn.position;
            bulletObject.transform.rotation = shootDir;
            bullet.SetProjectileLauncher(this);
            bullet.SetProjectileDamage(_dmg);
            bullet.SetTriggered(false);
            bullet.SetTheme(_theme);
            return bullet;
        }
        else
        {
            var bulletObject = Instantiate(_projectile, bulletSpawn.position, shootDir);
            var bullet = bulletObject.GetComponent<Projectile>();
            bullet.SetProjectileLauncher(this);
            bullet.SetProjectileDamage(_dmg);
            bullet.SetTheme(_theme);
            return bullet;
        }
        
    }

    public void Fire(Vector3 aimDir)
    {
        if (_timeToShoot == 0f && _ammo != 0)
        {
            _timeToShoot = _fireRate;
            var projectile = weaponConfig.bulletPrefab.gameObject;
            Quaternion shootDir = Quaternion.FromToRotation(projectile.transform.up, aimDir);
            GetProjectile(shootDir);
            _timeToAutomaticReload = weaponConfig.timeToAutomaticReload;
            if (_ammo > 0)
            {
                _ammo--;
            }
        }
        if (_ammo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void Fire(Quaternion shootDir)
    {
        if (_timeToShoot == 0f && _ammo != 0)
        {
            _timeToShoot = _fireRate;
            GetProjectile(shootDir);
            if (_ammo > 0)
            {
                _ammo--;
            }

        }
        if (_ammo == 0)
        {
            StartCoroutine(Reload());
        }
    }

    public void Fire(Vector3 aimDir, GameObject projectile)
    {
        Quaternion shootDir = Quaternion.FromToRotation(projectile.transform.up, aimDir);
        Instantiate(projectile, bulletSpawn.position, shootDir);
    }

    public void Enqueue(Projectile projectile)
    {
        if (!_hacked)
        {
            _projectilePool.Enqueue(projectile);
        }
        else
        {
            Destroy(projectile.gameObject);
        }
    }

    public IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
        _ammo = weaponConfig.ammo;
    }

    public void SetDamage(float dmg)
    {
        _dmg = dmg;
    }

    public void SetFireRate(float fireRate)
    {
        _fireRate = fireRate;
    }

    public void SetReloadTime(float reloadTime)
    {
        _reloadTime = reloadTime;
    }

    public void SetWeaponConfig(WeaponConfig weaponConfig)
    {
        this.weaponConfig = weaponConfig;
    }

    public void SetTheme(int theme)
    {
        _theme = theme;
    }

    public void HackProjectileLauncher(GameObject projectile)
    {
        _projectile = projectile;
        _hacked = true;
    }

    public void ResetAfterHack()
    {
        _hacked = false;
        _projectile = weaponConfig.bulletPrefab;
    }

    public GameObject Projectile => _projectile;

    public Transform BulletSpawn => bulletSpawn;

    public int Ammo => _ammo;

    public void Reset()
    {
        _dmg = weaponConfig.damage;
        _ammo = weaponConfig.ammo;
        _fireRate = weaponConfig.fireRate;
        _reloadTime = weaponConfig.reloadTime;
        _timeToShoot = 0;
        StopAllCoroutines();
    }

    public void ResetAfterAbility()
    {
        _dmg = weaponConfig.damage;
        _fireRate = weaponConfig.fireRate;
        _reloadTime = weaponConfig.reloadTime;
    }

    //TODO: Does not work; no idea why
    private void OnDestroy()
    {
        for (int i = 0; i < _projectilePool.Count; i++)
        {
            var projectile = _projectilePool.Dequeue();
            if(projectile != null)
                Destroy(projectile.gameObject);
        }
    }

    public float GetFireRate()
    {
        return _fireRate;
    }

    public void Slow(float amount)
    {
        _fireRate /= amount;
    }

    public void SlowReset()
    {
        _fireRate = weaponConfig.fireRate;
    }
}
