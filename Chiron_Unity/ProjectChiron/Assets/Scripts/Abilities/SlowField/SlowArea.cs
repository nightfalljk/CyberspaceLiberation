using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    [SerializeField] private SlowFieldConfig _slowFieldConfig;
    private List<EnemyBehaviour> inside = new List<EnemyBehaviour>();
    private List<Bullet> bulletsInside = new List<Bullet>();
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            //enemy.MoveSpeed *= _slowFieldConfig.moveSlowFactor;
            enemy.SlowEnemy(true, _slowFieldConfig.attackSlowFactor, _slowFieldConfig.moveSlowFactor);
            inside.Add(enemy);
        }

        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            bulletsInside.Add(bullet);
            bullet.Slowed(true, _slowFieldConfig.moveSlowFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyBehaviour>();
        if (enemy != null)
        {
            //enemy.MoveSpeed /= _slowFieldConfig.moveSlowFactor;
            enemy.SlowEnemy(false,0,0);
            inside.Remove(enemy);
        }
        
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            bulletsInside.Remove(bullet);
            bullet.Slowed(false, 0);
        }
    }

    private void OnDisable()
    {
        foreach (EnemyBehaviour enemy in inside)
        {
            //enemy.MoveSpeed /= _slowFieldConfig.moveSlowFactor;
            enemy.SlowEnemy(false,0,0);
        }
        inside.Clear();

        foreach (Bullet bullet in bulletsInside)
        {
            bullet.Slowed(false, 0);
        }
        bulletsInside.Clear();
    }
}
