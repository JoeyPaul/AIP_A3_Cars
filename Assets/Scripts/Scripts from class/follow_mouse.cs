using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow_mouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse_in_screen_coords = Input.mousePosition;
        Vector3 mouse_in_world_coords = Camera.main.ScreenToWorldPoint(mouse_in_screen_coords);
        transform.position = new Vector3(mouse_in_world_coords.x, mouse_in_world_coords.y, 0.0f);
    }
}
