using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Progression/ProgressionConfig")]
public class ProgressionConfig : ScriptableObject
{
    public float cooldownReductionFactor;
    public int cooldownReductionCost;
    public float maxCooldownReduction;

    public int secondLifeIncreaseCost;
    public int maxSecondLifeCharges;

    public float healthIncreaseAmount;
    public int healthIncreaseCost;
    public int maxHealthIncrease;

    public float damageIncreaseFactor;
    public int damageIncreaseCost;
    public float maxDamageIncrease;

    public float costScaling;
}
