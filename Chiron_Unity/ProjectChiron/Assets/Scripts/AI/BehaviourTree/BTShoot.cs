using System;

public class BTShoot : BTNode {

	public BTShoot(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        //return base.Execute(instanceID, init);
        Func<bool> func;
        if (Tree.GetBBValue<Func<bool>>(instanceID, "f.Shoot", out func))
        {
            func.Invoke();
            NodeState = Result.Success;
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