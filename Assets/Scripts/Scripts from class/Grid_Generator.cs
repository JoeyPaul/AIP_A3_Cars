using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Generator : MonoBehaviour
{
    public int width_of_world = 10;
    public int height_of_world = 10;
    public GameObject tile = null;

    void Start()
    {
        for(int y = 0; y < height_of_world; ++y)
        {
            for(int x = 0; x < width_of_world; ++x)
            {
                GameObject tile_instance = GameObject.Instantiate(tile);
                tile_instance.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
                tile_instance.name = "Tile_" + x.ToString() + "_" + y.ToString();

                tile_instance.GetComponent<Tile>().x = x;
                tile_instance.GetComponent<Tile>().y = y;
            }
        }
    }
}
