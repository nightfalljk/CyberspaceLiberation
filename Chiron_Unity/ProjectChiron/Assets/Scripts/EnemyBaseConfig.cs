using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/EnemyConfig")]
public class EnemyBaseConfig : EntityConfig
{
    [Header("cost of enemy stage, sorted from low to high")]
    public List<int> difficultyCost;
    
    [Header("Settings")]
    public float walkCooldown = 3;
    //public float laserDamage = 1;
    //public float laserDamageCooldown = 0.2f;
    public float boostedResistanceFactor = 0.5f;
    public float acceleration = 20;
}
