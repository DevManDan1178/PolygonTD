using UnityEngine;
using System;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake() 
    {  
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
    public void AddWaypoint(Transform _transform){
        points = (Transform[]) ArrayExpansion.AddToArrayEnd(_transform,points);
    }   
}