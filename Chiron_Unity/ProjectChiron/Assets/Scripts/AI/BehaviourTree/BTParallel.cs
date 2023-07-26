using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BTParallel : BTComposite
{
    public ResultSetting _ResultSetting { get; set; }

    public BTParallel(BehaviourTree bt, BTNode[] children) : base(bt, children)
    {
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        Result[] results = new Result[Children.Count];
        for (var i = 0; i < Children.Count; i++)
        {
            BTNode btNode = Children[i];
            results[i] = btNode.Execute(instanceID);
        }

        NodeState = GetResultBasedOnSetting(_ResultSetting, results);
        
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
    
}
