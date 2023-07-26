
using System.Collections.Generic;
using UnityEngine;

public class XNodeModifier : XNodeDecorator
{

    public BTNode.Modifier Modifier = BTNode.Modifier.Nothing;
    private BTNode.Modifier _lastModifier = BTNode.Modifier.Nothing;
    private void Reset()
	{
		name = "Modifier";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        List<BTNode> children = GetNextBtNodes(bt);
		
        if (children == null || children.Count < 1)
        {
            Debug.Log("No children given for "+this.GetType().Name);
            return CreateBtNode(bt, null);
        }
        btNode = new BTModifier(bt, children[0]);
        OnWriteValuesToBTNode();
        return btNode;
//        btNode = new BTModifier(bt);
//        OnWriteValuesToBTNode();
//        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTModifier bT_Modifier = btNode as BTModifier;
        bT_Modifier.Modifier = Modifier;
    }

    protected override bool CheckForNodeChanges()
    {
        bool change = false;
        //Check for value change of properties
        if (_lastModifier != Modifier)
        {
            change = true;
            _lastModifier = Modifier;
        }
        return change;
    }
    
    protected override bool OnCheckForConfigChanges()
    {
        bool change = false;
        //Check for value change of properties
        return change;
    }
    
    public override void OnLoadEntity(Entity entity)
    {
        base.OnLoadEntity(entity);
        if (entity == null)
        {
            //SetDefault
            return;
        }
        //Value = entity.GetParameter<float>(ValueName);
    }
    
    protected override void OnWriteConfigValues()
    {
        //entity.SetParameter(ValueName, Value);
    }
}