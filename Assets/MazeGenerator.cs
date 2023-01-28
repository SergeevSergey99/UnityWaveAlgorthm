using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wall, floor;
    public Material green, yellow, blue;
    int[,] pregenMaze = new int[10, 10]
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 0, 1, 1, 1, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
        {1, 0, 1, 1, 1, 0, 1, 1, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 1},
        {1, 0, 1, 0, 1, 1, 0, 0, 0, 1},
        {1, 0, 1, 1, 0, 0, 1, 0, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };

    MazeNode[,] Maze = new MazeNode[10, 10];

    public int START_X = 1, START_Z = 1;

    void ReColorMaze()
    {
        for(int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if(Maze[x,z].value == 0)
                {
                    if(x == START_X && z == START_Z)
                        Maze[x, z].GetComponent<MeshRenderer>().material = yellow;
                    else
                        Maze[x, z].GetComponent<MeshRenderer>().material = blue;
                }
            }

        }
    }

    void Start()
    {
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if (pregenMaze[x, z] == 0)
                {
                    var go = Instantiate(floor);
                    go.transform.position = 
                        new Vector3(x, go.transform.position.y, z);
                    go.GetComponent<MazeNode>().init(x, z, 0);
                    Maze[x, z] = go.GetComponent<MazeNode>();
                }
                if(pregenMaze[x, z] == 1)
                {
                    var go = Instantiate(wall);
                    go.transform.position =
                        new Vector3(x, go.transform.position.y, z);
                    go.GetComponent<MazeNode>().init(x, z, 1);
                    Maze[x, z] = go.GetComponent<MazeNode>();
                }
            }
        }

        ReColorMaze();
    }

    public void StartPathFinding(int x, int z)
    {
        ReColorMaze();
        WaveAlgorthm(START_X, START_Z, x, z);
    }

    public void WaveAlgorthm(int StartX, int StartZ, int EndX, int EndZ)
    {
        int[,] StepsArray = new int[10, 10];

        for (int x = 0; x < 10; x++)
            for (int z = 0; z < 10; z++)
                StepsArray[x, z] = int.MinValue;

        StepsArray[StartX, StartZ] = 0;
        List<MazeNode> queue = new List<MazeNode>();
        queue.Add(Maze[StartX, StartZ]);

        List<MazeNode> nextQueue = new List<MazeNode>();
        int stepCount = 1;

        while (queue.Count > 0)
        {
            var curr = queue[0];
            queue.RemoveAt(0);

            if (curr.x == EndX && curr.z == EndZ)
                break;

            if (curr.x - 1 >= 0
                && StepsArray[curr.x - 1, curr.z] == int.MinValue
                && pregenMaze[curr.x - 1, curr.z] == 0)
            {
                nextQueue.Add(Maze[curr.x - 1, curr.z]);
                StepsArray[curr.x - 1, curr.z] = stepCount;
            }
            if (curr.x + 1 < 10
                && StepsArray[curr.x + 1, curr.z] == int.MinValue
                && pregenMaze[curr.x + 1, curr.z] == 0)
            {
                nextQueue.Add(Maze[curr.x + 1, curr.z]);
                StepsArray[curr.x + 1, curr.z] = stepCount;
            }
            if (curr.z - 1 >= 0
                && StepsArray[curr.x, curr.z - 1] == int.MinValue
                && pregenMaze[curr.x, curr.z - 1] == 0)
            {
                nextQueue.Add(Maze[curr.x, curr.z - 1]);
                StepsArray[curr.x, curr.z - 1] = stepCount;
            }
            if (curr.z + 1 < 10
                && StepsArray[curr.x, curr.z + 1] == int.MinValue
                && pregenMaze[curr.x, curr.z + 1] == 0)
            {
                nextQueue.Add(Maze[curr.x, curr.z + 1]);
                StepsArray[curr.x, curr.z + 1] = stepCount;
            }

            if (queue.Count == 0)
            {
                queue.AddRange(nextQueue);
                nextQueue.Clear();
                stepCount++;
            }

        }

        if (StepsArray[EndX, EndZ] == int.MinValue)
        {
            Debug.Log("Невозможный путь");
        }
        else
        {
            var curr = Maze[EndX, EndZ];

            while (StepsArray[curr.x, curr.z] != 0)
            {
                curr.GetComponent<MeshRenderer>().material = green;

                if (curr.x - 1 >= 0
                    && StepsArray[curr.x - 1, curr.z]
                    == StepsArray[curr.x, curr.z] - 1)
                {
                    curr = Maze[curr.x - 1, curr.z];
                    continue;
                }
                if (curr.x + 1 < 10
                    && StepsArray[curr.x + 1, curr.z]
                    == StepsArray[curr.x, curr.z] - 1)
                {
                    curr = Maze[curr.x + 1, curr.z];
                    continue;
                }
                if (curr.z - 1 >= 0
                    && StepsArray[curr.x, curr.z - 1]
                    == StepsArray[curr.x, curr.z] - 1)
                {
                    curr = Maze[curr.x, curr.z - 1];
                    continue;
                }
                if (curr.z + 1 < 10
                    && StepsArray[curr.x, curr.z + 1]
                    == StepsArray[curr.x, curr.z] - 1)
                {
                    curr = Maze[curr.x, curr.z + 1];
                    continue;
                }

            }

        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (START_X - 1 >= 0
                && pregenMaze[START_X - 1, START_Z] == 0)
            {
                START_X--;
                ReColorMaze();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (START_X + 1 < 10
                && pregenMaze[START_X + 1, START_Z] == 0)
            {
                START_X++;
                ReColorMaze();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (START_Z + 1 < 10
                && pregenMaze[START_X, START_Z + 1] == 0)
            {
                START_Z++;
                ReColorMaze();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (START_Z - 1 >= 0
                && pregenMaze[START_X, START_Z - 1] == 0)
            {
                START_Z--;
                ReColorMaze();
            }
        }
    }
}
