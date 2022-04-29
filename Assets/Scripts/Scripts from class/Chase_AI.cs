using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_AI : MonoBehaviour
{
    Vector3 velocity_per_second = Vector3.zero;

    public float speed_per_second = 1.0f;

    public float steering_speed = 1.0f;

    void Update()
    {
        /*
         * A steering behaviour adds a steering vector per frame
         * which allows a agent to over time steer in the direction
         * that they want to be heading in, creating a more organic
         * and natural movement.
         */

        // Compute mouse position in world position.
        Vector3 mouse_position_in_world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse_position_in_world.z = 0.0f;

        // Compute our desired velocity.
        Vector3 desired_velocity_ps = mouse_position_in_world - transform.position;
        desired_velocity_ps.Normalize();
        desired_velocity_ps *= speed_per_second;

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
