using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour {

    public Transform waypointPrefab;
    public List<Waypoint> waypoints;

    /**
     * Return the closest waypoint to a given position in the World
     */
    public Waypoint getClosestWayPointToPosition (Vector3 position)
    {
        float closestDistance = -1;
        Waypoint closest = null;
        foreach(Waypoint waypoint in waypoints)
        {
            float distance = Vector3.Distance(waypoint.transform.position, position);
            if (distance < closestDistance || closestDistance == -1)
            {
                closestDistance = distance;
                closest = waypoint;
            }
        }
        return closest;
    }

    /**
     * Using A* find a path from a start waypoint to an end waypoint
     */
    public List<Waypoint> findPath (Waypoint start, Waypoint end)
    {
        List<Waypoint> openSet = new List<Waypoint>();
        List<Waypoint> closedSet = new List<Waypoint>();
        openSet.Add(start);
        while (openSet.Count > 0)
        {
            // Find the next optimal waypoint based on the distance to the start and the distance to the end
            Waypoint currentWaypoint = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentWaypoint.fCost || (openSet[i].fCost == currentWaypoint.fCost && openSet[i].hCost < currentWaypoint.hCost))
                {
                    currentWaypoint = openSet[i];
                }
            }

            // Make sure to check each waypoint only once
            openSet.Remove(currentWaypoint);
            closedSet.Add(currentWaypoint);

            // We made it, quit the loop and convert the found path to a waypoint list
            if (currentWaypoint == end)
            {
                return retracePath(start, currentWaypoint);
            }

            // Update the cost of each neighbor of the current waypoint, so that in the next loop the right decision can be made
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

    /**
     * Converts the parent waypoint relation (while figuring out the right path)
     * to a list of waypoint which an object can follow
     */
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

    /**
     * Creates a waypoint on a certain position
     */
    public void createWaypoint (Vector3 pos)
    {
        Transform p = Instantiate(waypointPrefab, pos, Quaternion.identity);
        p.parent = this.transform;
        waypoints.Add(p.GetComponent<Waypoint>());
    }

    /**
     * Connect the waypoint to eachother based on their distance to eachother
     * For now assume that if two tiles are next to eachother that they can be reached from eachother
     * This would bug out when there is a parallel road
     */
    public void connectWaypoints ()
    {
        foreach (Waypoint current in waypoints)
        {
            foreach (Waypoint other in waypoints)
            {
                if (Vector3.Distance(current.transform.position, other.transform.position) < World.tileSize + (World.tileSize / 4)) {
                    current.addConnection(other);
                }
            }
        }
    }
}
