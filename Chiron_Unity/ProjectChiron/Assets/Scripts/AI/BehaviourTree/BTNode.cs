using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BTNode
{
    public enum Result
    {
        Inactive,
        Running,
        Failure,
        Success,
    };


    public enum ResultSetting
    {
        AlwaysInactive,
        AlwaysRunning,
        AlwaysFailure,
        AlwaysSuccess,
        OneSuccess,
        AllSuccess,
        OneFailure,
        AllFailure,
        AllSuccessOrOneRunning,
        AllSuccessOrAllRunning
    };

    public enum ProcessType
    {
        SaveSequenceState,
        Condition
    }
    
    public enum Modifier
    {
        Nothing,
        Invert,
        AlwaysSuccess,
        AlwaysFailure,
        AlwaysRunning,
        NoFailure
    }
    
    private Result m_nodeState = Result.Inactive;
    public Result NodeState
    {
        get { return m_nodeState; }
        protected set { m_nodeState = value; }
    }
    public BehaviourTree Tree { get; set; }

    public BTNode(BehaviourTree bt)
    {
        Tree = bt;
    }

    public virtual Result Execute(int instanceID, bool init = false)
    {
        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

    public virtual void SetState(int instanceId, Result result)
    {
        Tree.SetStateValue(instanceId, this, result);
    }

    protected bool Exec(int instanceID, string ActionName)
    {
        Func<bool> func;
        if (Tree.GetBBValue<Func<bool>>(instanceID, ActionName, out func))
        {
            return func.Invoke();
        }
        return false;
    }
    
    protected bool Exec<T>(int instanceID, string ActionName, T a)
    {
        Func<T, bool> func;
        if (Tree.GetBBValue<Func<T, bool>>(instanceID, ActionName, out func))
        {
            return func. Invoke(a);
        }
        return false;
    }
    
    protected Result GetResultBasedOnSetting(ResultSetting resultSetting, Result[] results)
    {
        switch (resultSetting)
        {
            case ResultSetting.AlwaysFailure:
                return Result.Failure;
            
            case ResultSetting.AlwaysInactive:
                return Result.Inactive;
            
            case ResultSetting.AlwaysRunning:
                return Result.Running;
                
            case ResultSetting.AlwaysSuccess:
                return Result.Success;
                
            case ResultSetting.OneSuccess:
                if(results.Any(x => x==Result.Success))
                    return Result.Success;
                break;
            case ResultSetting.AllSuccess:
                return Result.Success;
                
            case ResultSetting.OneFailure:
                if(results.Any(x => x==Result.Failure))
                    return Result.Failure;
                break;
            case ResultSetting.AllFailure://Does this make sense?
                if(results.All(x => x == Result.Failure))
                    return Result.Failure;
                break;
            case ResultSetting.AllSuccessOrOneRunning:
                if (results.All(x => x == Result.Success))
                    return Result.Success;
                if(results.Any(x => x==Result.Running))
                    return Result.Running;
                break;
            case ResultSetting.AllSuccessOrAllRunning:
                if (results.All(x => x == Result.Success))
                    return Result.Success;
                if(results.All(x => x == Result.Running))
                    return Result.Running;
                break;
            default:
                Debug.LogWarning($"case {resultSetting} not defined");
                break;
        }

        return Result.Failure;
    }
}
