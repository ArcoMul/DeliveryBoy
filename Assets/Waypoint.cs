using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

    public List<Waypoint> connections;
    public bool walkable = true;
    public float hCost;
    public float gCost;
    public Waypoint parent;

    public float fCost
    {
        get
        {
            return hCost + gCost;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addConnection (Waypoint p)
    {
        connections.Add(p);
    }
}
