using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/SlowFieldConfig")]
public class SlowFieldConfig : ScriptableObject
{
    public GameObject slowProjectile;
    public float cooldown;
    public float duration;
    public float range;
    public float maxHeight;
    public float gravity;
    [Range(-180, 180)] public float firingAngle;
    [Range(0, 1)] public float moveSlowFactor;
    [Range(0, 1)] public float attackSlowFactor;
    public ReactiveProperty<int> tutExecutionCount = new ReactiveProperty<int>();
    public int tutConditionCount = 2;
    public ReactiveProperty<bool> tutCondition = new ReactiveProperty<bool>();

    public void Init(SlowFieldConfig slowFieldConfig)
    {
        this.slowProjectile = slowFieldConfig.slowProjectile;
        this.cooldown = slowFieldConfig.cooldown;
        this.duration = slowFieldConfig.duration;
        this.firingAngle = slowFieldConfig.firingAngle;
        this.attackSlowFactor = slowFieldConfig.attackSlowFactor;
        tutCondition.Value = false;
    }

    public void ResetTutCondition()
    {
        tutExecutionCount.Value = 0;
        tutCondition.Value = false;
    }
        
}
