using System;
using UnityEngine;
using UnityEngine.AI;

public class BTWalkTo : BTNode {
    
    private float distanceLeftOver = 1;
    
	public BTWalkTo(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        Result lastResult = Result.Inactive;
        if (Tree.GetStateValue(instanceID, this, out lastResult))
        {
            
        }
        if (init||lastResult == Result.Failure || lastResult==Result.Success || lastResult==Result.Inactive)
        {
            Vector3? dest = NextDestination(instanceID);
            SetDest(instanceID, dest);
            NodeState = Result.Running;
        }
        else
        {
            NavMeshAgent agent = null;
            if (Tree.GetBBValue<NavMeshAgent>(instanceID, "NavMeshAgent", out agent) && agent.gameObject.activeInHierarchy && agent.isOnNavMesh)
            {
                if (agent.remainingDistance < distanceLeftOver)
                {
                    NodeState = Result.Success;
                }
                else
                {
                    NodeState = Result.Running;
                }
            }
            else
            {
                NodeState = Result.Failure;
            }
        }
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }

    private void SetDest(int instanceId, Vector3? dest)
    {
        NavMeshAgent agent = null;
        if (Tree.GetBBValue<NavMeshAgent>(instanceId, "NavMeshAgent", out agent) &&
            agent.gameObject.activeInHierarchy &&
            agent.isOnNavMesh)
        {
            if (dest == null)
            {
                Debug.LogWarning($"No destination possible for {instanceId}");
            }
            else
            {
                agent.SetDestination(dest.Value);
            }
        }
    }

    private Vector3? NextDestination(int instanceId)
    {
        GameObject o;
        Tree.GetBBValue<GameObject>(instanceId, "This", out o);
        int nextDestinationIndex;
        Tree.GetBBValue<int>(instanceId, "nextDestinationIndex", out nextDestinationIndex);
        nextDestinationIndex++;
        Vector3? dest = o.GetComponent<PatrolList>()?.GetNext(ref nextDestinationIndex);
        Tree.SetBBValue(instanceId, "nextDestinationIndex", nextDestinationIndex);
        return dest;
    }
}