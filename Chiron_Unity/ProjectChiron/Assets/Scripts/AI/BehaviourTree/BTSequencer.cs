using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequencer : BTComposite
{
    public ProcessType _ProcessType { get; set; }

    public BTSequencer(BehaviourTree bt, BTNode[] children) : base(bt, children)
    {
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        int currentNode = 0;

        switch (_ProcessType)
        {
            case ProcessType.SaveSequenceState:
                currentNode = ExecuteSaveState(instanceID);
                break;
            case ProcessType.Condition:
                currentNode = ExecuteCondition(instanceID);
                break;
            default:
                Debug.LogError("ProcessType not defined in BTSequence");
                return Result.Failure;
                break;
                
        }

        if (NodeState != Result.Success)
        {
            //Set all after current to inactive
            for (int i = currentNode+1; i < Children.Count; i++)
            {
                Children[i].SetState(instanceID, Result.Inactive);
            }
        }
        Tree.SetBBValue(instanceID, "currentNode_"+ GetHashCode(), currentNode);
        
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

    private int ExecuteSaveState(int instanceID)
    {
        int currentNode = 0;
        Tree.GetBBValue(instanceID, "currentNode_" + GetHashCode(), out currentNode,true);
        if (currentNode < Children.Count)
        {
            Result resultChildren = Children[currentNode].Execute(instanceID);

            if (resultChildren == Result.Running)
            {
                NodeState = Result.Running;
            } else if(resultChildren == Result.Failure)
            {
                currentNode = 0;
                NodeState = Result.Failure;
            }
            else
            {
                currentNode++;
                if (currentNode < Children.Count)
                {
                    NodeState = Result.Running;
                }
                else
                {
                    currentNode = 0;
                    NodeState = Result.Success;
                }
            }
        }

        return currentNode;
    }

    private int ExecuteCondition(int instanceID)
    {
        int currentNode = 0;
        for (currentNode = 0; currentNode < Children.Count; currentNode++)
        {
            Result resultChild = Children[currentNode].Execute(instanceID);
            if (resultChild != Result.Success)
            {
                NodeState = resultChild;
                return currentNode;
            }
        }

        NodeState = Result.Success;
        return currentNode;
    }
}
