using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/DashConfig")]
public class DashConfig : ScriptableObject
{
    public float speed;
    public float cooldown;
    public float duration;
    public ReactiveProperty<int> tutExecutionCount = new ReactiveProperty<int>();
    public int tutConditionCount = 3;
    public ReactiveProperty<bool> tutCondition = new ReactiveProperty<bool>();

    public void Init(DashConfig dashConfig)
    {
        this.speed = dashConfig.speed;
        this.cooldown = dashConfig.cooldown;
        this.duration = dashConfig.duration;
        tutCondition.Value = false;
    }
    public void ResetTutCondition()
    {
        tutExecutionCount.Value = 0;
        tutCondition.Value = false;
    }
    
}
