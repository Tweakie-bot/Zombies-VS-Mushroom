using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    [SerializeField]
    private List<Transform> waypoints = new();

    public Transform GetWaypoint(int index)
    {
        if (index < 0 || index >= waypoints.Count)
        {
            return null;
        }

        return waypoints[index];
    }

    public int GetWaypointCount()
    {
        return waypoints.Count;
    }
}