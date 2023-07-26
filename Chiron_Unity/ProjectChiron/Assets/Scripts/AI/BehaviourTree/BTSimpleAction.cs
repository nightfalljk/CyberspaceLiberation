using System;

public class BTSimpleAction : BTNode
{
    public string ActionName { get; set; }

    public BTSimpleAction(BehaviourTree bt) : base(bt)
    {
    }

    public override Result Execute(int instanceID, bool init = false)
    {
        //return base.Execute(instanceID, init);
        Func<bool> func;
        if (Tree.GetBBValue<Func<bool>>(instanceID, ActionName, out func))
        {
            if (func.Invoke())
            {
                NodeState = Result.Success;
            }
            else
            {
                NodeState = Result.Failure;
            }
        }
        else
        {
            NodeState = Result.Failure;
        }
        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
}
