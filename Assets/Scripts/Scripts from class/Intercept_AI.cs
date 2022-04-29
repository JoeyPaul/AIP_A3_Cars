using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_AI : MonoBehaviour
{
    Vector3 velocity_per_second = Vector3.zero;
    public float speed_per_second = 3.0f;
    public Transform player;
    public float steering_speed = 1.0f;

    void Update()
    {
        player_movement_2 p_movement = player.GetComponent<player_movement_2>();

        // Compute the difference between our speeds.
        Vector3 relative_velocity = p_movement.velocity_per_second - velocity_per_second;
        
        // Compute distance to player.
        float distance = (transform.position - player.position).magnitude;

        // Compute how long until we intersect.
        float time_until_intersect = distance / (relative_velocity.magnitude);

        // Compute where player will be at the time of intersection.
        Vector3 predicted_player_pos = player.position + p_movement.velocity_per_second * time_until_intersect;

        Vector3 desired_velocity_ps = predicted_player_pos - transform.position;
        desired_velocity_ps.Normalize();
        desired_velocity_ps *= speed_per_second;

        //velocity_per_second = desired_velocity_ps;
        // transform.position += velocity_per_second * Time.deltaTime;

        /* ADD IN TURNING */

        // Compute steering vector for this frame.
        Vector3 steering_vector_ps = (desired_velocity_ps - velocity_per_second).normalized * steering_speed;
        Vector3 steering_vector_pf = steering_vector_ps * Time.deltaTime;

        // Determine new velocity direction with the steering added in.
        Vector3 steering_and_current = velocity_per_second + steering_vector_pf;
        steering_and_current.Normalize();

        // Determine new velocity with speed.
        velocity_per_second = steering_and_current * speed_per_second;

        transform.position += velocity_per_second * Time.deltaTime;

        
    }
}
