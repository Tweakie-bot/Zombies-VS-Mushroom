using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;

    [SerializeField]
    private float waypointReachDistance = 0.1f;

    private EnemyPath currentPath;
    private EnemyDamage enemyDamage;
    private EnemyWaveMember waveMember;

    private int currentWaypointIndex;
    private bool shouldMove;

    public void OnInitialize(EnemyPath path)
    {
        if (path == null || path.GetWaypointCount() == 0)
        {
            Debug.Log("No path is available");
            return;
        }
        currentPath = path;
        currentWaypointIndex = 0;

        enemyDamage = GetComponent<EnemyDamage>();
        waveMember = GetComponent<EnemyWaveMember>();

        shouldMove = true;
    }

    private void Update()
    {
        if (!shouldMove)
        {
            return;
        }

        MoveToNext();
    }

    private void MoveToNext()
    {
        Transform targetWaypoint = currentPath.GetWaypoint(currentWaypointIndex);

        if (targetWaypoint == null)
        {
            StopMoving();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementSpeed * Time.deltaTime);

        float remainingDistance = Vector3.Distance(transform.position,targetWaypoint.position);

        if (waypointReachDistance >= remainingDistance)
        {
            ReachCurrentWaypoint();
        }
    }

    private void ReachCurrentWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= currentPath.GetWaypointCount())
        {
            ReachEndOfPath();
        }
    }

    private void ReachEndOfPath()
    {
        StopMoving();

        EnemyWaveMember waveMember = GetComponent<EnemyWaveMember>();

        if (waveMember != null)
        {
            waveMember.ReachEndOfPath();
        }
    }

    private void StopMoving()
    {
        shouldMove = false;
    }
}