using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class BTRandomWalk : BTNode
{
    public WalkType walkType;
    public float minDistance;
    public float maxDistance;
    private float distanceLeftOver = 1;
    private float randomAroundPoint = 0.5f;

    public BTRandomWalk(BehaviourTree bt) : base(bt)
    {
    }

    private bool FindNextDestination(int instanceID)
    {
        if (Tree == null)
        {
            Debug.LogWarning("Tree null");
            return false;
        }
        if (Tree.Blackboard == null)
        {
            Debug.LogWarning("Blackboard null");
            return false;
        }
        
        bool found = false;
        NavMeshAgent agent;
        if (Tree.GetBBValue<NavMeshAgent>(instanceID, "NavMeshAgent", out agent))
        {
            Vector3 position = agent.gameObject.transform.position;
            Vector3 dest = position;
            List<Vector3> points;
            Tree.GetBBValue(0, "randomPoints", out points);
            switch (walkType)
            {
                case WalkType.SimpleSelf:
                    float dist = 25;
                    dest = RandomSimpleSelf(position, dist);
                    break;
                case WalkType.Self:
                    dest = SelectRandom(GetPoints(points, minDistance, maxDistance, position));
                    dest = RandomSimpleSelf(dest, randomAroundPoint);
                    break;
                case WalkType.CloseTarget:
                    Vector3 target;
                    Tree.GetBBValue(instanceID, "targetPosition", out target);
                    dest = SelectRandom(GetPoints(points, minDistance, maxDistance, target));
                    dest = RandomSimpleSelf(dest, randomAroundPoint);
                    break;
                case WalkType.RandomMap:
                    dest = SelectRandom(points);
                    dest = RandomSimpleSelf(dest, randomAroundPoint);
                    break;
                default:
                    break;
            }

            if (dest == Vector3.zero)
            {
                dest = RandomSimpleSelf(position, maxDistance);
            }
            
            if (agent.isOnNavMesh)
            {
                found = agent.SetDestination(dest);
            }
            else
            {
                found = false;
            }
        }
        else
        {
        }
        return found;
    }

    private List<Vector3> GetPoints(List<Vector3> points, float minDist, float maxDist, Vector3 position)
    {
        List<Vector3> finalpoints = new List<Vector3>();
        foreach (Vector3 vector3 in points)
        {
            float tmpDist = Vector3.Distance(position, vector3);
            if (tmpDist >= minDist && tmpDist <= maxDist)
            {
                finalpoints.Add(vector3);
            }
        }
        return finalpoints;
    }

    private Vector3 SelectRandom(List<Vector3> points)
    {
        if (points.Count == 0)
        {
            Debug.LogWarning("No points to select from");
            return Vector3.zero;
        }

        int selectedIndex = UnityEngine.Random.Range(0, points.Count);
        return points[selectedIndex];
    }
    
    private Vector3 RandomSimpleSelf(Vector3 pos, float dist)
    {
        float x = pos.x + (UnityEngine.Random.value * dist - dist/2.0f);
        float z = pos.z + (UnityEngine.Random.value * dist - dist/2.0f);
        return new Vector3(x, 0, z);
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        Result lastResult = Result.Inactive;
        if (Tree.GetStateValue(instanceID, this, out lastResult))
        {
            
        }
        if (init||lastResult == Result.Failure || lastResult==Result.Success || lastResult==Result.Inactive)
        {
            FindNextDestination(instanceID);
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

        //Tree.NodeStates[new Tuple<int, int>(instanceID, GetHashCode())] =  NodeState;
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
    
    
    public enum WalkType
    {
        SimpleSelf,
        RandomMap,
        CloseTarget,
        Self
    }
}
