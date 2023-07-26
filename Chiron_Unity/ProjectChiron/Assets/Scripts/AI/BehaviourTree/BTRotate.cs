using System;
using Unity.VisualScripting;
using UnityEngine;

public class BTRotate : BTNode
{

    public float angle;
    public float speed;
    
	public BTRotate(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        float deltaTime;
        float rotationSpeed = speed;//degree per second
        float alreadyRotated;
        float direction = angle >= 0 ? 1 : -1;
        Tree.GetBBValue<float>(0, "deltaTime", out deltaTime);
        Tree.GetBBValue(instanceID, "alreadyRotated_" + GetHashCode(), out alreadyRotated, true);
        //return base.Execute(instanceID, init);
        Func<float, bool> func;
        if (Tree.GetBBValue<Func<float, bool>>(instanceID, "f.SetRotate", out func))
        {
            //float percentdone = Mathf.InverseLerp(0, angle, alreadyRotated);
            float angleToRotateNow = rotationSpeed * deltaTime * direction;
            //prevent overrotation
            if (Mathf.Abs(alreadyRotated + angleToRotateNow) >= Mathf.Abs(angle))
            {
                angleToRotateNow = angle - alreadyRotated;
                NodeState = Result.Success;
                alreadyRotated = 0;
            }
            else
            {
                NodeState = Result.Running;
            }
            func.Invoke(angleToRotateNow);
            Tree.SetBBValue(instanceID, "alreadyRotated_" + GetHashCode(), alreadyRotated + angleToRotateNow);
        }
        else
        {
            NodeState = Result.Failure;
        }
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
}