using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UniRx;

public class InfectionBehaviour : EnemyBehaviour
{
    protected override void Awake()
    {
        base.Awake();
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


    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
//        functions.Add("f.TargetVisibleG",(Func<GameObject, bool>)(GetTargetVisibleG));
//        functions.Add("f.TargetVisible",(Func<bool>)(GetTargetVisible));
//        functions.Add("f.Target", (Func<GameObject>)(GetTarget));
//        functions.Add("f.Shoot",(Func<bool>)(Shoot));
//        functions.Add("f.CreateLaser",(Func<Transform,bool>)(CheckCreateLaser));
//        functions.Add("f.DeleteLaser",(Func<Transform, bool>)(DeleteLaser));
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
    
}
