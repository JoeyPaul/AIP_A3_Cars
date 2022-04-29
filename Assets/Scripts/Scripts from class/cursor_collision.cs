using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor_collision : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse_in_screen_coords = Input.mousePosition;
        Vector3 mouse_in_world_coords = Camera.main.ScreenToWorldPoint(mouse_in_screen_coords);

        bool is_between_lhs_and_rhs = mouse_in_world_coords.x >= button.lhs &&
            mouse_in_world_coords.x <= button.rhs;
        bool is_between_top_and_bot = mouse_in_world_coords.y >= button.bottom &&
            mouse_in_world_coords.y <= button.top;
        bool is_mouse_intersecting_button = is_between_lhs_and_rhs && is_between_top_and_bot;

        if (is_mouse_intersecting_button)
        {
            button.GetComponent<SpriteRenderer>().color = Color.magenta;
        }
        else
        {
            button.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
