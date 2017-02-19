using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public Transform roadPrefab;
    public WaypointManager waypointManager;

    string[,] tiles = new string[10, 10] {{"x","x","x","x","x","x","x","x","x","x"},
                                          {"x",".",".","x",".",".","x",".",".","x"},
                                          {"x",".",".","x",".",".","x",".","x","x"},
                                          {"x","x","x","x","x","x","x","x","x","."},
                                          {"x",".",".","x",".",".","x",".","x","."},
                                          {"x",".",".","x",".",".","x",".","x","."},
                                          {"x","x","x","x",".",".","x","x","x","x"},
                                          {"x",".",".",".",".",".","x",".",".","x"},
                                          {"x",".",".",".",".",".","x",".",".","x"},
                                          {"x","x","x","x","x","x","x","x","x","x"}};

    // Use this for initialization
    void Start () {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (tiles[y, x] == "x")
                {
                    Transform r = Instantiate(roadPrefab, new Vector3(x * 5, 0, y * 5), Quaternion.identity);
                    r.parent = this.transform;
                    waypointManager.createWaypoint(new Vector3(x * 5, 0, y * 5));
                }
            }
        }
        waypointManager.connectWaypoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
