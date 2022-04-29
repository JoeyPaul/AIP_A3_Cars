using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Modifier : MonoBehaviour
{
    public Color wall_color;
    public Color tile_color;
    public Color start_color;
    public Color end_color;
    public Pathfinder pathfinder;

    void Update()
    {
        Vector2 mouse_position_in_screen_coords = Input.mousePosition;
        Vector3 mouse_position_in_world_coords = Camera.main.ScreenToWorldPoint(mouse_position_in_screen_coords);
        mouse_position_in_world_coords.z = 0.0f;

        int mouse_position_tile_coords_x = (int)mouse_position_in_world_coords.x;
        int mouse_position_tile_coords_y = (int)mouse_position_in_world_coords.y;
        GameObject selected_tile = GameObject.Find("Tile_" + mouse_position_tile_coords_x.ToString() + "_" + mouse_position_tile_coords_y.ToString());

        bool player_pressed_rmb = Input.GetMouseButtonDown(1);
        bool turn_tile_to_wall = player_pressed_rmb;
        if (turn_tile_to_wall)
        {
            bool tile_is_not_wall = !selected_tile.GetComponent<Tile>().is_wall;
            if(tile_is_not_wall)
            {
                SpriteRenderer selected_tile_SR = selected_tile.GetComponent<SpriteRenderer>();
                selected_tile_SR.color = wall_color;
                selected_tile.GetComponent<Tile>().is_wall = true;
            }
            else
            {
                SpriteRenderer selected_tile_SR = selected_tile.GetComponent<SpriteRenderer>();
                selected_tile_SR.color = tile_color;
                selected_tile.GetComponent<Tile>().is_wall = false;
            }
        }

        bool player_pressed_lmb = Input.GetMouseButtonDown(0);
        if(player_pressed_lmb)
        {
            // setting the start position
            if (pathfinder.start == null)
            {
                selected_tile.GetComponent<SpriteRenderer>().color = start_color;
                pathfinder.start = selected_tile.GetComponent<Tile>();
            }
            // setting the end position
            else if (pathfinder.end == null)
            {
                selected_tile.GetComponent<SpriteRenderer>().color = end_color;
                pathfinder.end = selected_tile.GetComponent<Tile>();

                pathfinder.FindPath();
            }
            // resetting with a new start position
            else if(pathfinder.start != null && pathfinder.end != null)
            {
                Tile[] tiles = FindObjectsOfType<Tile>();
                foreach (Tile tile in tiles)
                {
                    if(!tile.is_wall)
                        tile.GetComponent<SpriteRenderer>().color = tile_color;
                }

                pathfinder.start = null;
                pathfinder.end = null;

                selected_tile.GetComponent<SpriteRenderer>().color = start_color;
                pathfinder.start = selected_tile.GetComponent<Tile>();
            }
        }

    }
}
