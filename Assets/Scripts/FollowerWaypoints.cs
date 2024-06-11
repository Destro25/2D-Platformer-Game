using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerWaypoints : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypoint = 0;

    [SerializeField] private float speed = 2f;

    private void Update()
    {
        Vector2 currentWaypointPosition = waypoints[currentWaypoint].transform.position;

        if (Vector2.Distance(currentWaypointPosition, transform.position) < 0.1f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        transform.position = Vector2.MoveTowards(transform.position, currentWaypointPosition, Time.deltaTime * speed);
    }
}
