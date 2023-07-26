using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class ArmBehaviour : EnemyBehaviour
{

//    [SerializeField] private Transform bulletSpawnpoint2;
//    [SerializeField] private Transform bulletSpawnpoint3;
//    [SerializeField] private Transform bulletSpawnpoint4;
//    [SerializeField] private Transform bulletSpawnpoint5;
//    [SerializeField] private Transform bulletSpawnpoint6;
    [SerializeField] private LineRenderer LineRenderer;

    private GameObject boss;
    
    protected override void Awake()
    {
        base.Awake();
        NavMeshAgent.updateRotation = false;
    }

    private void OnEnable()
    {
        boss = FindObjectOfType<BossBehaviour>().gameObject;
    }

    protected override void LoadConfig()
    {
        base.LoadConfig();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        LineRenderer.SetPositions(new []{boss.transform.position, transform.position });
    }

    public override bool Show()
    {
        LineRenderer.enabled = true;
        return base.Show();
    }

    public override bool Hide()
    {
        LineRenderer.enabled = false;
        return base.Hide();
    }

    public bool SetRotate(float angle)
    {
        gameObject.transform.Rotate(Vector3.up, angle);
        return true;
    }

    public override Dictionary<string, object> GetFunctions()
    {
        Dictionary<string, object> functions = new Dictionary<string, object>(base.GetFunctions());
        functions.Add("f.SetRotate",(Func<float, bool>)(SetRotate));
        //functions.Add("f.Show");
        return functions;
    }
    
}
