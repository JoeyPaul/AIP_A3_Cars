using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement_2 : MonoBehaviour
{
    public Vector3 velocity_per_second = Vector3.zero;
    public float speed_per_second = 4.0f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 desired_velocity = new Vector3(horizontal, vertical, 0.0f);
        desired_velocity.Normalize();
        desired_velocity *= speed_per_second;

        velocity_per_second = desired_velocity;

        transform.position += velocity_per_second * Time.deltaTime;
    }
}
