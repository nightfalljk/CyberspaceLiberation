using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UniRx;

public class BossBehaviour : EnemyBehaviour
{
    [SerializeField] private GameObject ArmsPrefab;
    
    private AiDirector aiDirector;
    [Range(0,1)]
    [SerializeField] private float reactivateShieldPercentage = 0.4f;
    //[ColorUsage(true, true)] [SerializeField] private Color shieldActiveColor;
    //[ColorUsage(true, true)] [SerializeField] private Color shieldInactiveColor;
    [SerializeField] private Material shieldMaterial;
    [SerializeField] private Material invisivbleMaterial;
    [SerializeField] private int shieldMaterialIndex;
    //[SerializeField] private MeshRenderer meshRenderer;
    //[SerializeField] private Collider innerCollider;
    private bool shieldActive;

    private bool armsSpawned;

    private bool shieldReactive = false;
    //private int tmpMinionTokens;

    protected override void Awake()
    {
        base.Awake();
        ActivateShield(true);
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

    }


    public bool SpawnArms()
    {
        aiDirector.SpawnArms(ArmsPrefab);
        stage++;
        ActivateShield(true);
        armsSpawned = true;
        return true;
    }
    
    public bool SpawnMinions()
    {
        aiDirector.SpawnMinions();
        return true;
    }

    public void ActivateShield(bool toggle)
    {
        shieldActive = toggle;
        //shieldMaterial.SetColor("_col", toggle ? shieldActiveColor : shieldInactiveColor);
        //_meshRenderer.mater
        Material[] materials = (Material[]) _meshRenderer.materials.Clone();
        materials[shieldMaterialIndex] = toggle ? shieldMaterial : invisivbleMaterial;
        _meshRenderer.materials = materials;
        _collider.enabled = toggle;
    }

    public override bool TakeDamage(float damage)
    {
        if (shieldActive)
            return false;
        bool result = base.TakeDamage(damage);
        if (Health.Value <= config.maxHealth * reactivateShieldPercentage && !shieldReactive)
        {
            ActivateShield(true);
            stage++;
            shieldReactive = true;
        }
        return result;
    }

    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
//        functions.Add("f.TargetVisibleG",(Func<GameObject, bool>)(GetTargetVisibleG));
//        functions.Add("f.TargetVisible",(Func<bool>)(GetTargetVisible));
//        functions.Add("f.Target", (Func<GameObject>)(GetTarget));
//        functions.Add("f.Shoot",(Func<bool>)(Shoot));
//        functions.Add("f.CreateLaser",(Func<Transform,bool>)(CheckCreateLaser));
        functions.Add("f.SpawnArms",(Func<bool>)(SpawnArms));
        functions.Add("f.SpawnMinions",(Func<bool>)(SpawnMinions));
        return functions;
        //return base.GetFunctions();
    }


    public override bool Hide()
    {
        bool b = base.Hide();
        return b;
    }

    public override bool Shoot()
    {
        bool b = base.Shoot();
        TriggerAnimation("shoot");
        return b;
    }
//    public override Dictionary<string, object> GetParameters()
//    {
//        Dictionary<string, object> parameters = new Dictionary<string, object>(base.GetParameters());
//        //parameters.Add("Health", Health);
//        parameters.Add("ShootingCooldown", ShootingCooldown);
//        return parameters;
//    }

    public void ConectAiDirector(AiDirector aiDirector)
    {
        this.aiDirector = aiDirector;
    }

    public void AllArmsDead()
    {
        if(!armsSpawned)
            return;
        ActivateShield(false);
        stage++;
        armsSpawned = false;
    }
}
