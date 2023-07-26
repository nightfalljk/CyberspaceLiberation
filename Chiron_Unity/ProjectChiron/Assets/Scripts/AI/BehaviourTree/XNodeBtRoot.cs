using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using XNode;
//using XNode.Examples.StateGraph;
using Object = UnityEngine.Object;

public class XNodeBtRoot : XNodeBT
{
    [Output] public Empty start;

    public string EntityName;
    [SerializeField] private bool found; //As feedback
    public EntityConfig EntityConfig;

    [HideInInspector]
    public Entity Entity;
    
    private string lastEntityName;
    private EntityConfig lastEntityConfig = null;

    private void Reset()
    {
        name = "Root";
    }

    
    protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        List<BTNode> children = GetNextBtNodes(bt);
        if (children == null || children.Count < 1)
        {
            Debug.Log("No children given for "+this.GetType().Name);
            return null;
        }
        return new BTDecorator(bt, children[0]);
    }

    public void SetIdWithName(string name)
    {
        GameObject go = GameObject.Find(name);
        XNodeGraph graphX = graph as XNodeGraph;
        if (go != null)
        {
            EnemyBehaviour eb = go.GetComponent<EnemyBehaviour>();
            //Entity = eb;
            //int id = eb.GetHashCode();
            //graphX.ChangeViewingInstanceID(id);
            SetEntity(eb);
            found = true;
        }
        else
        {
            //graphX.ChangeViewingInstanceID(0);
            SetEntity(null);
            //Entity = null;
            found = false;
        }
    }

    //setting by script; do no confuse with WriteConfigValues
    public void SetEntity(Entity entity)
    {
        if (entity == null)
        {
            EntityConfig = null;
            Entity = null;
            entity = null;//this?
        }
        else
        {
            Entity = entity;
            EntityConfig = entity.GetConfig();
        }
        SetEntity();
    }
    
    //setting in editor; do no confuse with WriteConfigValues
    public void SetEntity()
    {
        //load into graph
        XNodeGraph graphX = graph as XNodeGraph;
        graphX.LoadEntity(Entity);
        //en
    }

    public void UpdateEntity()
    {
        SetIdWithName(EntityName);
    }
    
    protected override bool CheckForNodeChanges()
    {
        bool change = false;
        if (lastEntityName != EntityName)
        {
            lastEntityName = EntityName;
            SetIdWithName(EntityName);
            change = true;
        }
        if (EntityConfig != lastEntityConfig)
        {
            lastEntityConfig = EntityConfig;
            if(entity!=null)
                entity.SetConfig(EntityConfig);
            SetEntity();
            change = true;
        }
        return change;
    }

    protected override bool OnCheckForConfigChanges()
    {
        return base.OnCheckForConfigChanges();
    }
	
    public override void OnLoadEntity(Entity entity)
    {
        base.OnLoadEntity(entity);
        if (entity == null)
        {
            //SetDefault
            return;
        }
        //Set values from Config
    }
	
    protected override void OnWriteConfigValues()
    {
    }
}

[Serializable]
public class Empty { }