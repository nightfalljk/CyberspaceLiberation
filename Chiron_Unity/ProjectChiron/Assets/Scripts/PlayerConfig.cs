using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(menuName = "config/PlayerConfig")]
public class PlayerConfig : EntityConfig
{
    //Already in base class 
//    public float WalkCooldown = 0;
//    public bool canDie = false;
//    public float maxHealth = 100;
//    public float ShootingCooldown = 4;
//    public float moveSpeed = 4;

    public void Init(PlayerConfig playerConfig)
    {
        
        this.canDie = playerConfig.canDie;
        this.maxHealth = playerConfig.maxHealth;
        //this.shootingCooldown = playerConfig.shootingCooldown;
        this.moveSpeed = playerConfig.moveSpeed;
    }
}
