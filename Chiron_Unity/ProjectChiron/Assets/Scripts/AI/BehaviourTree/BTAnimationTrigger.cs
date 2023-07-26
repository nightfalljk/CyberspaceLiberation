using System;

public class BTAnimationTrigger : BTNode {

    public string TriggerName { get; set; }
    
	public BTAnimationTrigger(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        //return base.Execute(instanceID, init);
        Func<string, bool> func;
        if (Tree.GetBBValue<Func<string, bool>>(instanceID, "f.TriggerAnimation", out func))
        {
            if (func.Invoke(TriggerName))
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
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

}