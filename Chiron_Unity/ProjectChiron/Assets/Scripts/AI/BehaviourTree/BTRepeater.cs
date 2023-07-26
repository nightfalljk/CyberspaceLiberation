using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

/// <summary>
/// Returns success after child returned x times success.
/// Returns running in between.
/// Returns failure if child returns failure and resets counter.
/// </summary>
public class BTRepeater : BTDecorator
{
    public int RepeatXTimes { get; set; }

    public BTRepeater(BehaviourTree bt, BTNode child) : base(bt, child)
    {
        
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        int counter = 0;
        Tree.GetBBValue(instanceID, "counter_" + GetHashCode(), out counter,true);
        
        
        if (counter < RepeatXTimes)
        {
            Result childResult = Child.Execute(instanceID);
            if (childResult == Result.Failure)
            {
                NodeState = Result.Failure;
                counter = 0;
            }
            else if (childResult == Result.Running)
            {
                NodeState = Result.Running;
            }
            else if (childResult == Result.Success)
            {
                NodeState = Result.Running;
                counter++;
            }
        }
        
        if(counter == RepeatXTimes)
        {
            NodeState = Result.Success;
            counter = 0;
        }
        
        Tree.SetBBValue(instanceID, "counter_"+ GetHashCode(), counter);
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
}
