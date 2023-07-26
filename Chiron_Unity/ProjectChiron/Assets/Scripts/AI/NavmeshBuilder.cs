using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshBuilder : MonoBehaviour, IManager
{
    private NavMeshSurface[] surfaces;
    public bool enableOnStart = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (enableOnStart)
        {
            BuildNavMeshes();
        }
    }

    private void BuildNavMeshes()
    {
        if (surfaces == null || surfaces.Length == 0)
        {
            Debug.LogWarning("No surfaces to build Navmesh");
            return;
        }
        Debug.Log("Building NavMeshes");
        foreach (NavMeshSurface navMeshSurface in surfaces)
        {
            if (navMeshSurface == null)
            {
                Debug.LogWarning("No NavMeshSurface on surface");
                continue;
            }
            navMeshSurface.BuildNavMesh();
        }
    }

    public void ResetLevel()
    {
        if (surfaces == null)
            return;
        foreach (NavMeshSurface navMeshSurface in surfaces)
        {
            if (navMeshSurface == null)
            {
                Debug.LogWarning("suface has no navMeshSurface");
                continue;
            }
            //navMeshSurface.BuildNavMesh();
            navMeshSurface.RemoveData();
        }
    }

    public void StartLevel()
    {
        BuildNavMeshes();
    }

    public void SetNavmeshSurfaces(List<NavMeshSurface> surfaces)
    {
        this.surfaces = surfaces.ToArray();
    }
}
