using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    // Maximums.
    public float maximum_speed_per_second = 1.0f;
    public float rotation_speed_per_second = 90.0f;
    public float acceleration_per_second = 3.0f;

    // Velocity.
    public float current_direction_degrees = 90.0f;
    public float current_velocity_per_second = 0.0f;

    //public FlowFieldManager ffm;
    public MapLoader ml;

    void Update()
    {
        Vector3 force_direction = Vector3.zero;
        Rigidbody rb = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.W))
        {
            force_direction = new Vector3(Mathf.Cos(current_direction_degrees * Mathf.Deg2Rad), Mathf.Sin(current_direction_degrees * Mathf.Deg2Rad), 0.0f);

            // Forward
            Vector3 front_of_vehicle = transform.position + transform.up;
            Debug.DrawRay(front_of_vehicle, force_direction, Color.red);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            force_direction = new Vector3(Mathf.Cos(current_direction_degrees * Mathf.Deg2Rad), Mathf.Sin(current_direction_degrees * Mathf.Deg2Rad), 0.0f);
            force_direction *= -1;

            // Backward
            Vector3 back_of_vehicle = transform.position - transform.up;
            Debug.DrawRay(back_of_vehicle, force_direction, Color.red);
        }

        #region FlowField
        // FLOW FIELD STUFF
        //Vector3 force = ffm.GetForceForPosition(transform.position);
        //force_direction += force;
        //force_direction.Normalize();
        //rb.AddForce(force_direction * acceleration_per_second * Time.deltaTime, ForceMode.Acceleration);
        #endregion

        MapLoader.Tile_Type type = ml.GetSpeedForLocation(transform.position);

        float acceleration_scalar = 0.0f;
        switch (type)
        {
            case MapLoader.Tile_Type.ASPHALT:
                //rb.drag = 0.33f;
                acceleration_scalar = ml.tiles["ASPHALT"];
                break;
            case MapLoader.Tile_Type.MUD:
                //rb.drag = 1.0f;
                acceleration_scalar = ml.tiles["MUD"];
                break;
            case MapLoader.Tile_Type.DIRT:
                //rb.drag = 0.5f;
                acceleration_scalar = ml.tiles["DIRT"];
                break;
            case MapLoader.Tile_Type.DEFAULT:
                //rb.drag = 0.33f;
                acceleration_scalar = ml.tiles["DEFAULT"];
                break;
        }

        // Left
        Debug.DrawRay(transform.position, -transform.right, Color.yellow);

        // Right
        Debug.DrawRay(transform.position, transform.right, Color.blue);

        // Current Velocity
        Debug.DrawRay(transform.position, rb.velocity, new Color(0.7f, 0.0f, 1.0f));

        /*
         * This assessment requires a physically based controller -- this is not using
         * the physics system as we are directly mutating the rotation.
         */
        #region Rotation
        if (Input.GetKey(KeyCode.A))
        {
            current_direction_degrees += rotation_speed_per_second * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            current_direction_degrees -= rotation_speed_per_second * Time.deltaTime;
        }
        Vector3 rotation = new Vector3(0.0f, 0.0f, current_direction_degrees - 90.0f);
        transform.eulerAngles = rotation;
        #endregion



        #region Velocity
        if (Input.GetKey(KeyCode.W))
        {
            current_velocity_per_second += acceleration_per_second * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            current_velocity_per_second -= acceleration_per_second * Time.deltaTime;
        }
        current_velocity_per_second = Mathf.Clamp(current_velocity_per_second, -maximum_speed_per_second, maximum_speed_per_second);
        #endregion

        Vector3 velocity_per_second = new Vector3(Mathf.Cos(current_direction_degrees * Mathf.Deg2Rad), Mathf.Sin(current_direction_degrees * Mathf.Deg2Rad), 0.0f);
        velocity_per_second *= current_velocity_per_second;

        transform.position += velocity_per_second * acceleration_scalar * Time.deltaTime;


    }
}
