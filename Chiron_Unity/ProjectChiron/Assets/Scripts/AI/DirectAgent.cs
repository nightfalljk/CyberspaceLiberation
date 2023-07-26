using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Update = UnityEngine.PlayerLoop.Update;

public class DirectAgent : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private GameObject player;
    [SerializeField] private float movementCooldown = 5;
    [SerializeField] private float fightingDistance = 2;
    
    private float movementTimer = 0;
    void Start() 
    {
        agent = GetComponent<NavMeshAgent> ();
        agent.updateRotation = false;
    }

    void Update()
    {
        movementTimer -= Time.deltaTime;
        transform.LookAt(player.transform, Vector3.up);
        if (movementTimer <= 0)
        {
            MoveToLocation(PointBetweenPlayerAndEnemy(fightingDistance));
            movementTimer += movementCooldown;
        }
    }
    
    public bool MoveToLocation(Vector3 targetPoint)
    {
        if (!agent.isOnNavMesh)
        {
            Debug.Log("Agent"+transform.name+" not on a Navmesh");
            return false;
        }

        agent.SetDestination(targetPoint);
        //agent.destination = targetPoint;
        agent.isStopped = false;
        return true;
    }

    Vector3 PointBetweenPlayerAndEnemy(float distance)
    {
        return player.transform.position += (transform.position - player.transform.position).normalized * distance;
    }

}
