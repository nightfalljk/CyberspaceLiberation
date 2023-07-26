using System;
using UnityEngine;
using XNode;

public class BTModifier : BTDecorator
{

    public Modifier Modifier = Modifier.Nothing;
    
	public BTModifier(BehaviourTree bt, BTNode child) : base(bt, child)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        //return base.Execute(instanceID, init);
//        Func<bool> func;
//        if (Tree.GetBBValue<Func<bool>>(instanceID, ActionName, out func))
//        {
//            func.Invoke();
//            NodeState = Result.Success;
//        }
//        else
//        {
//            NodeState = Result.Failure;
//        }
        Result childState = Child.Execute(instanceID, init);
        NodeState = childState;
        switch (Modifier)
        {
            case Modifier.Nothing:
                break;
            case Modifier.Invert:
                if (childState == Result.Failure)
                {NodeState = Result.Success;}
                else if(childState == Result.Success)
                {NodeState = Result.Failure;}
                break;
            case Modifier.AlwaysSuccess:
                NodeState = Result.Success;
                break;
            case Modifier.AlwaysFailure:
                NodeState = Result.Failure;
                break;
            case Modifier.AlwaysRunning:
                NodeState = Result.Running;
                break;
            case Modifier.NoFailure:
                NodeState = childState;
                if (childState == Result.Failure)
                    NodeState = Result.Success;
                break;
            default:
                Debug.LogWarning("Modifier behaviour undefined");
                break;
        }

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