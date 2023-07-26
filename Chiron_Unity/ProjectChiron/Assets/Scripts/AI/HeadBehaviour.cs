using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class HeadBehaviour : EnemyBehaviour
{

    [SerializeField] private Transform bulletSpawnpoint2;
    [SerializeField] private Transform bulletSpawnpoint3;
    [SerializeField] private Transform bulletSpawnpoint4;
    [SerializeField] private Transform bulletSpawnpoint5;
    [SerializeField] private Transform bulletSpawnpoint6;

    protected override void Awake()
    {
        base.Awake();
        NavMeshAgent.updateRotation = false;
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override bool Shoot()
    {
        if (!IsAlive.Value)
        {
            Debug.LogWarning("Shooting while dead");
            return false;
        }
        ShootAllOnce();
        
        return true;
    }
    private void ShootAllOnce()
    {
        Vector3 centerBulletSpawn = Vector3.zero;
        centerBulletSpawn += bulletSpawnpoint.position;
        centerBulletSpawn += bulletSpawnpoint2.position;
        centerBulletSpawn += bulletSpawnpoint3.position;
        centerBulletSpawn += bulletSpawnpoint4.position;
        centerBulletSpawn += bulletSpawnpoint5.position;
        centerBulletSpawn += bulletSpawnpoint6.position;
        centerBulletSpawn /= 6;
        
        Quaternion shootingDirection1 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint.position.x, centerBulletSpawn.y,bulletSpawnpoint.position.z)- centerBulletSpawn);
        Quaternion shootingDirection2 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint2.position.x, centerBulletSpawn.y,bulletSpawnpoint2.position.z)- centerBulletSpawn);
        Quaternion shootingDirection3 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint3.position.x, centerBulletSpawn.y,bulletSpawnpoint3.position.z)- centerBulletSpawn);
        Quaternion shootingDirection4 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint4.position.x, centerBulletSpawn.y,bulletSpawnpoint4.position.z)- centerBulletSpawn);
        Quaternion shootingDirection5 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint5.position.x, centerBulletSpawn.y,bulletSpawnpoint5.position.z)- centerBulletSpawn);
        Quaternion shootingDirection6 = Quaternion.FromToRotation(bulletPrefab.transform.up, new Vector3(bulletSpawnpoint6.position.x, centerBulletSpawn.y,bulletSpawnpoint6.position.z)- centerBulletSpawn);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint;
        _projectileLauncher.Fire(shootingDirection1);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint2;
        _projectileLauncher.Fire(shootingDirection2);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint3;
        _projectileLauncher.Fire(shootingDirection3);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint4;
        _projectileLauncher.Fire(shootingDirection4);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint5;
        _projectileLauncher.Fire(shootingDirection5);
        _projectileLauncher.bulletSpawn = bulletSpawnpoint6;
        _projectileLauncher.Fire(shootingDirection6);
    }

    public bool SetRotate(float angle)
    {
        gameObject.transform.Rotate(Vector3.up, angle);
        return true;
    }

    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
        functions.Add("f.SetRotate",(Func<float, bool>)(SetRotate));
        //functions.Add("f.Show");
        return functions;
    }
    
}
