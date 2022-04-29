using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool is_wall = false;
    public int x;
    public int y;
    public float distance_from_start = 0.0f;
    public bool visited = false;
    public List<Tile> shortest_path_to_start = new List<Tile>();
}
