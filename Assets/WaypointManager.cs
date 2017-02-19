using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour {

    public Transform waypointPrefab;
    public List<Waypoint> waypoints;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Waypoint getClosestWayPointToPosition (Vector3 position)
    {
        // Debug.Log("getClosestWayPointToPosition: " + position);
        float closestDistance = -1;
        Waypoint closest = null;
        foreach(Waypoint waypoint in waypoints)
        {
            float distance = Vector3.Distance(waypoint.transform.position, position);
            // Debug.Log("Waypoint: " + waypoint.name + " - " + position + " vs. " + waypoint.transform.position + " - " + distance);
            if (distance < closestDistance || closestDistance == -1)
            {
                closestDistance = distance;
                closest = waypoint;
            }
        }
        // Debug.Log("closest: " + closest.name);
        return closest;
    }

    public List<Waypoint> findPath (Waypoint start, Waypoint end)
    {
        List<Waypoint> openSet = new List<Waypoint>();
        List<Waypoint> closedSet = new List<Waypoint>();
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            Waypoint currentWaypoint = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentWaypoint.fCost || (openSet[i].fCost == currentWaypoint.fCost && openSet[i].hCost < currentWaypoint.hCost))
                {
                    currentWaypoint = openSet[i];
                }
            }

            openSet.Remove(currentWaypoint);
            closedSet.Add(currentWaypoint);

            if (currentWaypoint == end)
            {
                return retracePath(start, currentWaypoint);
            }

            foreach (Waypoint neighbor in currentWaypoint.connections)
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentWaypoint.gCost + Vector3.Distance(currentWaypoint.transform.position, neighbor.transform.position);
                if (newMovementCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = Vector3.Distance(neighbor.transform.position, end.transform.position);
                    neighbor.parent = currentWaypoint;
                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        return null;
    }

    List<Waypoint> retracePath (Waypoint start, Waypoint end)
    {
        List<Waypoint> list = new List<Waypoint>();
        Waypoint current = end;
        while (current != start)
        {
            list.Add(current);
            current = current.parent;
        }
        list.Add(start);
        list.Reverse();
        return list;
    }

    public void createWaypoint (Vector3 pos)
    {
        Transform p = Instantiate(waypointPrefab, pos, Quaternion.identity);
        p.parent = this.transform;
        waypoints.Add(p.GetComponent<Waypoint>());
    }

    public void connectWaypoints ()
    {
        foreach (Waypoint current in waypoints)
        {
            foreach (Waypoint other in waypoints)
            {
                if (Vector3.Distance(current.transform.position, other.transform.position) < 6) {
                    current.addConnection(other);
                }
            }
        }
    }
}
