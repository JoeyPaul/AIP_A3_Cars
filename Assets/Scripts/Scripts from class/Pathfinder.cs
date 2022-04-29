using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public Tile start = null;
    public Tile end = null;
    public Color visited_color;
    public Color path_color;

    public void FindPath()
    {
        // Reset all tiles for a new path.
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach(Tile tile in tiles)
        {
            tile.distance_from_start = float.PositiveInfinity;
            tile.visited = false;
            tile.shortest_path_to_start = new List<Tile>();
        }

        // Initialize start.
        start.distance_from_start = 0;

        // Initialize unvisited.
        HashSet<Tile> unvisited_tiles = new HashSet<Tile>(tiles);

        Tile current = start;
        while(current != end)
        {
            // Find all tiles that surrount the current tile.
            GameObject LL = GameObject.Find("Tile_" + (current.x - 1) + "_" + (current.y + 0));
            GameObject TL = GameObject.Find("Tile_" + (current.x - 1) + "_" + (current.y + 1));
            GameObject TT = GameObject.Find("Tile_" + (current.x - 0) + "_" + (current.y + 1));
            GameObject TR = GameObject.Find("Tile_" + (current.x + 1) + "_" + (current.y + 1));
            GameObject RR = GameObject.Find("Tile_" + (current.x + 1) + "_" + (current.y + 0));
            GameObject BR = GameObject.Find("Tile_" + (current.x + 1) + "_" + (current.y - 1));
            GameObject BB = GameObject.Find("Tile_" + (current.x + 0) + "_" + (current.y - 1));
            GameObject BL = GameObject.Find("Tile_" + (current.x - 1) + "_" + (current.y - 1));

            // Set the shortest path to the surrounding tiles.
            if (LL) SetShortestPathToTileFromTile(current, LL.GetComponent<Tile>(), current.distance_from_start + 1.0f);
            if (TL) SetShortestPathToTileFromTile(current, TL.GetComponent<Tile>(), current.distance_from_start + 1.4f);
            if (TT) SetShortestPathToTileFromTile(current, TT.GetComponent<Tile>(), current.distance_from_start + 1.0f);
            if (TR) SetShortestPathToTileFromTile(current, TR.GetComponent<Tile>(), current.distance_from_start + 1.4f);
            if (RR) SetShortestPathToTileFromTile(current, RR.GetComponent<Tile>(), current.distance_from_start + 1.0f);
            if (BR) SetShortestPathToTileFromTile(current, BR.GetComponent<Tile>(), current.distance_from_start + 1.4f);
            if (BB) SetShortestPathToTileFromTile(current, BB.GetComponent<Tile>(), current.distance_from_start + 1.0f);
            if (BL) SetShortestPathToTileFromTile(current, BL.GetComponent<Tile>(), current.distance_from_start + 1.4f);

            // We've visited the current tile.
            current.visited = true;
            unvisited_tiles.Remove(current);

            // Color the current tile as visited.
            if(current != start && current != end)
                current.GetComponent<SpriteRenderer>().color = visited_color;

            // Find the tile with the smallest distance from start.
            // All the tiles that surround the current tile will be in this list.
            Tile smallest_distance_from_start = null;
            foreach(Tile unvisited_tile in unvisited_tiles)
            {
                if(smallest_distance_from_start == null || smallest_distance_from_start.distance_from_start > unvisited_tile.distance_from_start)
                {
                    smallest_distance_from_start = unvisited_tile;
                }
            }

            // Now start this process again from the new tile, until we find the end.
            current = smallest_distance_from_start;
        }

        // We've found a path, color it.
        foreach(Tile tile_in_path in end.shortest_path_to_start)
        {
            if (tile_in_path != start && tile_in_path != end)
                tile_in_path.GetComponent<SpriteRenderer>().color = path_color;
        }
    }

    void SetShortestPathToTileFromTile(Tile from, Tile to, float distance_from_start)
    {
        // Ignore the shortest path if a path is already defined that is shorter
        // or the tile is a wall.
        if(to.distance_from_start > distance_from_start && !to.is_wall)
        {
            to.distance_from_start = distance_from_start;
            to.shortest_path_to_start = new List<Tile>(from.shortest_path_to_start);
            to.shortest_path_to_start.Add(from);
        }
    }
}
