using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{   
    [HideInInspector]
    public Transform target;
    public int waypointIndex = 0;
    private Enemy enemy;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        target = Waypoints.points[0];
    }
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        if (dir.magnitude <= enemy.speed * Time.deltaTime) //if the movement left is less than the frame step, just move that movement
        {
            transform.position = target.position;
        } else
        {
            transform.position += dir.normalized * enemy.speed * Time.deltaTime;
        }
    
        if((transform.position - target.position).sqrMagnitude <= 0.0025f)
        {
            GetNextWaypoint();
        }
    }

    void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints.points.Length -1 )
        {
            enemy.EndPath();
            return;
        }

        waypointIndex ++;
        target = Waypoints.points[waypointIndex];
    }
}
