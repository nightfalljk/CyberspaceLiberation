using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu]
public class EntityConfig : ScriptableObject
{
    //public float WalkCooldown = 0;
    public bool canDie = false;
    public float maxHealth = 100;
    //public float shootingCooldown = 4;
    public float moveSpeed = 4;
}
