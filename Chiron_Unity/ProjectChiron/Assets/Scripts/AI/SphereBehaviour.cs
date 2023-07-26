using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UniRx;

public class SphereBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject LaserPrefab;

    private Dictionary<Transform, LineRenderer> _lineRenderers = new Dictionary<Transform, LineRenderer>();

    private float laserDamageCooldown = 0;
    [SerializeField] private bool boosted;

    protected override void Awake()
    {
        base.Awake();
        //_projectileLauncher = GetComponent<ProjectileLauncher>();
        IsAlive.Where(x => x == false).Subscribe(b =>
        {
            Transform[] t = _lineRenderers.Keys.ToArray();
            for (int i = _lineRenderers.Count - 1; i >= 0; i--)
            {
                DeleteLaser(t[i]);
            }
        });
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
//        EnemyBaseConfig ebc = (EnemyBaseConfig) config;
//        WalkCooldown = ebc.walkCooldown;
        //shootingTimer = config.ShootingCooldown;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        boosted = _lineRenderers.Count > 0;
        foreach (KeyValuePair<Transform,LineRenderer> keyValuePair in _lineRenderers)
        {
            UpdateLaser(keyValuePair.Value,keyValuePair.Key);
            
            Entity e;
//            if (CheckLaserDamage(keyValuePair.Key, out e) && e != null)
//            {
//                //LaserDamage(e);
//            }
        }
    }

    public override bool TakeDamage(float damage)
    {
        EnemyBaseConfig c = config as EnemyBaseConfig;
        float factor = boosted ? 1-c.boostedResistanceFactor : 1;
        return base.TakeDamage(damage * factor);
    }

//    private Vector3 GetTargetDirection(Transform t)
//    {
//        return (t.transform.position - bulletSpawnpoint.position).normalized;
//    }
    
//    private bool CheckLaserDamage(Transform otherSphere, out Entity entity)
//    {
//        Vector3 direction = GetTargetDirection(otherSphere.transform);
//        RaycastHit hit;
//        entity = null;
//        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position,otherSphere.position), visibilityMask))
//        {
//            GameObject go = hit.collider.gameObject;
//            entity = hit.collider.gameObject.GetComponent<Entity>();
//            if (go != null && go == target.gameObject && entity != null)
//            {
//                return true;
//            }
//        }
//        return false;
//    }

//    private void LaserDamage(Entity entity)
//    {
//        EnemyBaseConfig c = config as EnemyBaseConfig;
//        laserDamageCooldown -= Time.deltaTime;
//        if (laserDamageCooldown <= 0)
//        {
//            entity.TakeDamage(c.laserDamage);
//            laserDamageCooldown = c.laserDamageCooldown;
//        }
//    }

    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
//        functions.Add("f.TargetVisibleG",(Func<GameObject, bool>)(GetTargetVisibleG));
//        functions.Add("f.TargetVisible",(Func<bool>)(GetTargetVisible));
//        functions.Add("f.Target", (Func<GameObject>)(GetTarget));
//        functions.Add("f.Shoot",(Func<bool>)(Shoot));
        functions.Add("f.CreateLaser",(Func<Transform,bool>)(CheckCreateLaser));
        functions.Add("f.DeleteLaser",(Func<Transform, bool>)(DeleteLaser));
        return functions;
        //return base.GetFunctions();
    }

    public bool CheckCreateLaser(Transform t)
    {
        LineRenderer lr;
        if (!_lineRenderers.ContainsKey(t))// !_lineRenderers.TryGetValue(transform, out lr))
        {
            CreateLaser(out lr);
            _lineRenderers.Add(t,lr);
            UpdateLaser(lr, t);
            return true;
        }
        return false;
    }

    private bool CreateLaser(out LineRenderer lr)
    {
        GameObject LaserGo = Instantiate(LaserPrefab, transform);
        lr = LaserGo.GetComponent<LineRenderer>();
        return true;
    }
    private void UpdateLaser(LineRenderer lr, Transform t)
    {
        lr.SetPositions(new Vector3[]{transform.position, t.position});
    }

    public bool DeleteLaser(Transform t)
    {
        LineRenderer lr;
        if (_lineRenderers.TryGetValue(t, out lr))
        {
            _lineRenderers.Remove(t);
            Destroy(lr.gameObject);
            return true;
        }
        return false;
    }

    public override bool Hide()
    {
        bool b = base.Hide();
        return b;
    }

    public override bool Shoot()
    {
        bool b = base.Shoot();
        TriggerAnimation("shoot");
        return b;
    }
//    public override Dictionary<string, object> GetParameters()
//    {
//        Dictionary<string, object> parameters = new Dictionary<string, object>(base.GetParameters());
//        //parameters.Add("Health", Health);
//        parameters.Add("ShootingCooldown", ShootingCooldown);
//        return parameters;
//    }
    
}
