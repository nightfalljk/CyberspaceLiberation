using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolList : MonoBehaviour
{
    [SerializeField] private List<Transform> destinations = new List<Transform>();
    

    public Vector3 GetNext(ref int nextDestinationIndex)
    {
        nextDestinationIndex %= destinations.Count;
        return destinations[nextDestinationIndex].position;
    }
}
