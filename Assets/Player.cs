using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public WaypointManager waypointManager;
    public Transform target;

    Vector3 movingGoal;
    Vector3 movingStartPosition;
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


	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) {
            onMouseClick();
        }

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

    void moveTo (Vector3 _goal)
    {
        Debug.Log("moveTo " + _goal);
        movingGoal = _goal;
        target.position = movingGoal;
        state = "moving";
        currentPath = waypointManager.findPath(waypointManager.getClosestWayPointToPosition(this.transform.position), waypointManager.getClosestWayPointToPosition(movingGoal));
        if (currentPath == null)
        {
            Debug.LogError("No path found");
        }
        else
        {
            Debug.Log("Found path:");
            foreach(Waypoint waypoint in currentPath)
            {
                Debug.Log(waypoint.name);
            }
        }
    }
}
