using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionSystem : MonoBehaviour
{

    //TODO: Add configs; adapt configs as needed when player buys stuff; initialize new configs or do they stay around
    [SerializeField] private PlayerCharacterController player;
    [SerializeField] private PlayerConfig originalPlayerConfig;
    [SerializeField] private WeaponConfig originalPlayerWeaponConfig;
    [SerializeField] private ProgressionConfig progressionConfig;
    
    [SerializeField] private DashConfig originalDashConfig;
    [SerializeField] private TeleportConfig originalTeleportConfig;
    [SerializeField] private WeaponBoostConfig originalWeaponBoostConfig;
    [SerializeField] private SecondLifeConfig originalSecondLifeConfig;
    [SerializeField] private SlowFieldConfig originalSlowFieldConfig;
    [SerializeField] private HackConfig originalHackConfig;

    private PlayerConfig _playerConfig;
    private DashConfig _dashConfig;
    private TeleportConfig _teleportConfig;
    private WeaponBoostConfig _weaponBoostConfig;
    private SecondLifeConfig _secondLifeConfig;
    private SlowFieldConfig _slowFieldConfig;
    private HackConfig _hackConfig;
    
    public static int currency;

    private float _currentCooldownReductionFactor;
    private float _currentDamageIncrease;

    private void Awake()
    {
        currency = 0;

        _playerConfig = ScriptableObject.CreateInstance<PlayerConfig>();
        _playerConfig.Init(originalPlayerConfig);
        _dashConfig = ScriptableObject.CreateInstance<DashConfig>();
        _dashConfig.Init(originalDashConfig);
        _teleportConfig = ScriptableObject.CreateInstance<TeleportConfig>();
        _teleportConfig.Init(originalTeleportConfig);
        _weaponBoostConfig = ScriptableObject.CreateInstance<WeaponBoostConfig>();
        _weaponBoostConfig.Init(originalWeaponBoostConfig);
        _secondLifeConfig = ScriptableObject.CreateInstance<SecondLifeConfig>();
        _secondLifeConfig.Init(originalSecondLifeConfig);
        _slowFieldConfig = ScriptableObject.CreateInstance<SlowFieldConfig>();
        _slowFieldConfig.Init(originalSlowFieldConfig);
        _hackConfig = ScriptableObject.CreateInstance<HackConfig>();
        _hackConfig.Init(originalHackConfig);

    }

    public void DecreaseCooldown()
    {
        if (currency >= progressionConfig.cooldownReductionCost
            && _currentCooldownReductionFactor < progressionConfig.maxCooldownReduction)
        {
            _currentCooldownReductionFactor += progressionConfig.cooldownReductionFactor;
            
            _dashConfig.cooldown *= 1 - (_currentCooldownReductionFactor);
            _teleportConfig.cooldown *= 1 - (_currentCooldownReductionFactor);
            _weaponBoostConfig.cooldown *= 1 - (_currentCooldownReductionFactor);
            _slowFieldConfig.cooldown *= 1 - (_currentCooldownReductionFactor);
            _hackConfig.cooldown *= 1 - (_currentCooldownReductionFactor);

            currency -= progressionConfig.cooldownReductionCost;
        }
    }

    public void IncreaseSecondLifeCharges()
    {
        if (currency >= progressionConfig.secondLifeIncreaseCost
            && _secondLifeConfig.charges < progressionConfig.maxSecondLifeCharges)
        {
            currency -= progressionConfig.secondLifeIncreaseCost;
            _secondLifeConfig.charges++;
        }
    }

    public void IncreaseHealth()
    {
        throw new NotImplementedException();
    }

    public void IncreaseDamage()
    {
        if (currency >= progressionConfig.damageIncreaseCost
            && _currentDamageIncrease < progressionConfig.maxDamageIncrease)
        {
            _currentDamageIncrease += progressionConfig.damageIncreaseFactor;
            //TODO: Figure out where to apply this

            currency -= progressionConfig.damageIncreaseCost;
        }
        throw  new NotImplementedException();
    }

}
