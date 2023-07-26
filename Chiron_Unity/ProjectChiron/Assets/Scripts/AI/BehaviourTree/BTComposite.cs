using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTComposite : BTNode
{
    public List<BTNode> Children { get; set; }

    public BTComposite(BehaviourTree bt, BTNode [] children) : base(bt)
    {
        Children = new List<BTNode>(children);
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        Debug.LogWarning("BTComposite execute called: no effect");
        NodeState = Result.Failure;
        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

    public override void SetState(int instanceId, Result result)
    {
        base.SetState(instanceId, result);
        foreach (BTNode child in Children)
        {
            child.SetState(instanceId, result);
        }
    }
}
