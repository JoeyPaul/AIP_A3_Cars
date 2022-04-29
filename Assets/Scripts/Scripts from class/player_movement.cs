using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    public float speed = 1.0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement_direction = new Vector3(horizontal, vertical, 0.0f);
        movement_direction.Normalize();

        Vector3 movement_this_frame = movement_direction * speed * Time.deltaTime;

        transform.position += movement_this_frame;
    }
}
