using System;
using XNode;

public class BTWait : BTNode
{

    public string WaitVariableName;
    public float WaitValue;
    
	public BTWait(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        float timeSinceStart = 0;
        float timeToWait = 0;
        if (string.IsNullOrEmpty(WaitVariableName))
        {
            timeToWait = WaitValue;
        }
        else
        {
            Tree.GetBBValue<float>(instanceID, WaitVariableName, out timeToWait);
        }
        float timeWhenWaitStarted = 0;

        Tree.GetBBValue(instanceID, "waitStart_" + GetHashCode(), out timeWhenWaitStarted, true);
        
        //Fail if value not retrieved
        if (Tree.GetBBValue<float>(0, "timeSinceStart", out timeSinceStart))
        {
            //Check if new wait started based on last state
            Result lastResult;
            Tree.GetStateValue(instanceID, this, out lastResult);
            if (lastResult == Result.Inactive || lastResult == Result.Failure || lastResult == Result.Success)
            {
                timeWhenWaitStarted = timeSinceStart;
                Tree.SetBBValue(instanceID, "waitStart_" + GetHashCode(), timeWhenWaitStarted);
                NodeState = Result.Running;
            }
            //Check if wait finished
            if (timeSinceStart > timeWhenWaitStarted+timeToWait)
            {
                //Result result = Child.Execute(instanceID,true);
                //lastWait = timeSinceStart;
                //Tree.SetBBValue(instanceID, "lastWait_"+GetHashCode(), lastWait);
                
                NodeState = Result.Success;
            }
            else
            {
                //Result result = Child.Execute(instanceID);
                NodeState = Result.Running;
            }
        }
        else
        {
            NodeState = Result.Failure;
        }
        
        //NodeState = Result.Running;
        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
}