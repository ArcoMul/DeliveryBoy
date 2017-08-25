using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    public static int tileSize = 4;
    public static int worldSize = 10;
    public WaypointManager waypointManager;

    // Different types of road prefabs
    public Transform roadStraight;
    public Transform roadTSplit;
    public Transform roadCrossway;
    public Transform roadCurve;

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

    void Start ()
    {
        createWorld();
        waypointManager.connectWaypoints();
	}

    /**
     * Creates a world of tiles based on a string array
     * x means road
     * . means empty
     */
    void createWorld ()
    {
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (tiles[y, x] == "x")
                {
                    // Figure out what kind of neighor tiles this tile has
                    bool top = false, right = false, bottom = false, left = false;
                    if (y > 0 && tiles[y - 1, x] == "x")
                    {
                        top = true;
                    }
                    if (y < worldSize - 1 && tiles[y + 1, x] == "x")
                    {
                        bottom = true;
                    }
                    if (x > 0 && tiles[y, x - 1] == "x")
                    {
                        left = true;
                    }
                    if (x < worldSize - 1 && tiles[y, x + 1] == "x")
                    {
                        right = true;
                    }

                    // Instantiate the right prefab based on which neighbor there are
                    Transform road = roadStraight;
                    float angle = 0;
                    string name = "none";
                    if (top && right && bottom && left)
                    {
                        road = roadCrossway;
                        name = "top-right-bottom-left";
                    }
                    else if (top && right && bottom)
                    {
                        road = roadTSplit;
                        name = "top-right-bottom";
                    }
                    else if (top && right && left)
                    {
                        road = roadTSplit;
                        angle = -90;
                        name = "top-right-left";
                    }
                    else if (top && bottom && left)
                    {
                        road = roadTSplit;
                        angle = 180;
                        name = "top-bottom-left";
                    }
                    else if (right && bottom && left)
                    {
                        road = roadTSplit;
                        angle = 90;
                        name = "right-bottom-left";
                    }
                    else if (top && bottom)
                    {
                        road = roadStraight;
                        name = "top-bottom";
                    }
                    else if (top && right)
                    {
                        road = roadCurve;
                        name = "top-right";
                    }
                    else if (top && left)
                    {
                        road = roadCurve;
                        angle = -90;
                        name = "top-left";
                    }
                    else if (bottom && right)
                    {
                        road = roadCurve;
                        angle = 90;
                        name = "bottom-right";
                    }
                    else if (bottom && left)
                    {
                        road = roadCurve;
                        angle = 180;
                        name = "bottom-left";
                    }
                    else if (left && right)
                    {
                        road = roadStraight;
                        angle = 90;
                        name = "left-right";
                    }

                    Transform r = Instantiate(road, new Vector3(x * tileSize, 0, -y * tileSize), Quaternion.Euler(new Vector3(0, angle, 0)));
                    r.name = name;
                    r.parent = this.transform;
                    waypointManager.createWaypoint(new Vector3(x * tileSize, 0, -y * tileSize));
                }
            }
        }
    }
}
