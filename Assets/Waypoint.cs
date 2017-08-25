using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public List<Waypoint> connections;

    // Option to dynamically make a waypoint inaccessible
    public bool walkable = true;

    // Previous step in the at this moment calculated route
    public Waypoint parent;

    // Distance from waypoint to final waypoint
    public float hCost;

    // Total distance cost from start to this waypoint
    public float gCost;

    // Total cost of this waypoint
    public float fCost
    {
        get
        {
            return hCost + gCost;
        }
    }

    public void addConnection (Waypoint p)
    {
        connections.Add(p);
    }
}
