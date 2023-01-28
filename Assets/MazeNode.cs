using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode : MonoBehaviour
{
    public int x;
    public int z;
    public int value;

    public void init(int X, int Z, int V)
    {
        x = X;
        z = Z;
        value = V;
    }

    private void OnMouseDown()
    {
        FindObjectOfType<MazeGenerator>().StartPathFinding(x, z);
    }
}
