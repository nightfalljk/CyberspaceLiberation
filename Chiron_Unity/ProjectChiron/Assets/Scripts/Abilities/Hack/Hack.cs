using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;

public class Hack : MonoBehaviour
{

    [SerializeField] private AiDirector aiDirector;
    private HackConfig _hackConfig;
    private ReactiveProperty<bool> _hackEnabled;
    private List<EnemyBehaviour> _enemies;
    private EnemyBehaviour _hackedTarget;
    private ReactiveProperty<bool> _hackAvailable;
    private ReactiveProperty<float> _hackCooldownPercentage;
    private ReactiveProperty<float> _hackDurationPercentage;
    private ReactiveProperty<bool> _enemiesInRange;

    private EnemyBehaviour closest;

    private SphereCollider collider;
    //private GameObject closestTarget = null;
    private bool hackPossible;
    
    private void Awake()
    {
        _enemies = new List<EnemyBehaviour>();
        _hackAvailable = new ReactiveProperty<bool>();
        _hackEnabled = new ReactiveProperty<bool>();
        _hackCooldownPercentage = new ReactiveProperty<float>();
        _hackDurationPercentage = new ReactiveProperty<float>();
        _enemiesInRange = new ReactiveProperty<bool>();
        _enemiesInRange.Value = false;
        _hackCooldownPercentage.Value = 1;
        _hackDurationPercentage.Value = 1;
        _hackAvailable.Value = true;
        _hackedTarget = null;

        collider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        foreach (Entity entity in aiDirector.GetEnemies(true))
        {
            entity.hackable.Value = false;
        }

        _enemies.Clear();
        
        foreach (Entity entity in aiDirector.GetEnemies(true))
        {
            if (Vector3.Distance(transform.position, entity.transform.position) <= collider.radius)
            {
                _enemies.Add(entity as EnemyBehaviour);
            }
        }

        _enemiesInRange.Value = _enemies.Count > 0;
        
        closest = GetClosestEnemy();
        hackPossible = CheckHackPossible();
        if (hackPossible)
        {
            closest.hackable.Value = true;
        }
    }

    public void SetHackConfig(HackConfig hackConfig)
    {
        _hackConfig = hackConfig;
    }

    private bool CheckHackPossible()
    {
        if (closest == null)
        {
            return false;
        }
        
        GameObject closestTarget = aiDirector.GetClosest(closest)?.gameObject;

        if (closestTarget == null)
        {
            return false;
        }

        return true;
    }
    
    public bool HackEnemy(GameObject projectile)
    {
        if (!hackPossible)
            return false;
        _hackedTarget = closest;
        _hackAvailable.Value = false;
        _hackedTarget.ProjectileLauncher.HackProjectileLauncher(projectile);
        aiDirector.HackEnemy(_hackedTarget.gameObject);

        StartCoroutine(Duration());
        return true;

    }

    private EnemyBehaviour GetClosestEnemy()
    {
        if (_enemies.Count == 0)
            return null;
        
        int closest = 0;
        float closestDist = -1;
        for (int i = 0; i < _enemies.Count; i++)
        {
            var dist = (gameObject.transform.position - _enemies[i].gameObject.transform.position).magnitude;
            if (closestDist < 0)
            {
                closestDist = dist;
                closest = i;
            }
            else if (dist < closestDist)
            {
                closestDist = dist;
                closest = i;
            }
        }

        return _enemies[closest];
    }

    private void ResetEnemy()
    {
        _hackedTarget.ProjectileLauncher.ResetAfterHack();
        aiDirector.ResetHack(_hackedTarget.gameObject);
        _hackedTarget = null;
        //_enemies.Clear();
    }

//    private void OnTriggerEnter(Collider other)
//    {
//        var enemy = other.gameObject.GetComponent<EnemyBehaviour>();
//        //TODO: Catch boss
//        if (enemy != null)
//        {
//            if (_enemies.Count == 0)
//                _enemiesInRange.Value = true;
//            _enemies.Add(enemy);
//        }
//        
//    }
//
//    private void OnTriggerExit(Collider other)
//    {
//        var enemy = other.gameObject.GetComponent<EnemyBehaviour>();
//        //TODO: Catch boss
//        if (enemy != null)
//        {
//            _enemies.Remove(enemy);
//            if (_enemies.Count == 0)
//                _enemiesInRange.Value = false;
//        }
//    }

    public ReactiveProperty<bool> HackAvailable
    {
        get => _hackAvailable;
        set => _hackAvailable = value;
    }

    public ReactiveProperty<bool> HackEnabled
    {
        get => _hackEnabled;
        set => _hackEnabled = value;
    }

    public ReactiveProperty<float> HackCooldownPercentage => _hackCooldownPercentage;

    public ReactiveProperty<float> HackDurationPercentage => _hackDurationPercentage;
    
    private IEnumerator Duration()
    {
        yield return new WaitForDuration(_hackConfig.duration, _hackDurationPercentage);
        ResetEnemy();
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForCooldown(_hackConfig.cooldown, _hackCooldownPercentage);
        _hackAvailable.Value = true;
    }

    public void ResetOnNewLevel()
    {
        _hackAvailable.Value = true;
        _hackCooldownPercentage.Value = 1;
        _hackDurationPercentage.Value = 1;
        _hackedTarget = null;
        StopAllCoroutines();
        _enemies.Clear();
        _enemiesInRange.Value = false;
    }

    public ReactiveProperty<bool> EnemiesInRange => _enemiesInRange;
}
