using System;
using System.Collections.Generic;
using UnityEngine;

public class BTLasers : BTNode
{

    public float _MinDistance = 0;
    
	public BTLasers(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        GameObject instance;
        Tree.GetBBValue(instanceID, "This", out instance);
        List<GameObject> spheres;
        if (Tree.GetBBValue(0, "Spheres",out spheres))
        {
            int c = 0;
            foreach (GameObject sphere in spheres)
            {
                int sphereId = sphere.GetComponent<Entity>().GetInstanceID(); //? same as gameObject.GetInstanceID();?
                if (sphereId != instanceID)
                {
                    float dist = Mathf.Abs(Vector3.Distance(instance.transform.position, sphere.transform.position));
                    if (dist < _MinDistance)
                    {
                        if (Exec(instanceID, "f.TargetVisibleG", sphere))
                        {
                            Exec(instanceID, "f.CreateLaser", sphere.transform);
                            //NodeState = Result.Success;
                            c++;
                        }
                        else
                        {
                            Exec(instanceID, "f.DeleteLaser", sphere.transform);
                            //NodeState = Result.Failure;
                        }
                    }
                    else
                    {
                        Exec(instanceID, "f.DeleteLaser", sphere.transform);
                        //NodeState = Result.Failure;
                    }
                }
            }

            if (c > 0) NodeState = Result.Success;
            else {NodeState = Result.Failure; }
        }
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
}