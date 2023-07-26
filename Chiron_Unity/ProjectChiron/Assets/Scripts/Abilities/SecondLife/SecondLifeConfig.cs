using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "config/Abilities/SecondLifeConfig")]
public class SecondLifeConfig : ScriptableObject
{

    public float rebirthHealth;
    public int charges;

    public void Init(SecondLifeConfig secondLifeConfig)
    {
        this.rebirthHealth = secondLifeConfig.rebirthHealth;
        this.charges = secondLifeConfig.charges;
    }
}
