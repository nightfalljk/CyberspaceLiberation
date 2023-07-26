using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDecorator : BTNode
{
    public BTNode Child { get; set; }

    public BTDecorator(BehaviourTree bt, BTNode child) : base(bt)
    {
        Child = child;
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        if (Child == null)
            return NodeState;
        NodeState = Child.Execute(instanceID);
        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

    public override void SetState(int instanceId, Result result)
    {
        base.SetState(instanceId, result);
        if(Child!=null)
            Child.SetState(instanceId,result);
    }
}
