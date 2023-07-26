using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/HackConfig")]
public class HackConfig : ScriptableObject
{
    public float cooldown;
    public float duration;

    public void Init(HackConfig hackConfig)
    {
        this.cooldown = hackConfig.cooldown;
        this.duration = hackConfig.duration;
    }
}
