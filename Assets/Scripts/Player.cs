using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public WaypointManager waypointManager;

    // If the player is moving, this is the path to follow
    List<Waypoint> currentPath;

    string _state;
    string state
    {
        get
        {
            return _state;
        }
        set
        {
            Debug.Log("Player: change state: " + value);
            _state = value;
        }
    }


	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) {
            onMouseClick();
        }

        // While moving, move to the next point in the path the player is following
        if (state == "moving")
        {
            Waypoint nextWaypoint = currentPath[0];
            Vector3 direction = nextWaypoint.transform.position - this.transform.position;
            this.transform.Translate(direction.normalized / 10);
            if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) < 0.05)
            {
                currentPath.RemoveAt(0);
            }
            if (currentPath.Count == 0)
            {
                state = "idle";
            }
        }
    }

    /**
     * Find where the user clicked, translate to world position, start moving the player there
     */
    void onMouseClick ()
    {
        Plane plane = new Plane(Vector3.up, 0);
        float dist;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out dist))
        {
            Vector3 point = ray.GetPoint(dist);
            moveTo(point);
        }
    }

    /**
     * Move the player to a certain goal position, following the waypoints.
     */
    void moveTo (Vector3 _goal)
    {
        state = "moving";
        currentPath = waypointManager.findPath(waypointManager.getClosestWayPointToPosition(this.transform.position), waypointManager.getClosestWayPointToPosition(_goal));
    }
}
