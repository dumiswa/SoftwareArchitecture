using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to save all the waypoints
public class Waypoints : MonoBehaviour
{
    public static Waypoints Instance { get; private set; }
    public Transform[] points;

    //Implemented as a singleton in order for the enmies to access this class and the required waypoint
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = transform.GetChild(i);
        }

    }

}
