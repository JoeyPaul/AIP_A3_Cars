using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wander_AI : MonoBehaviour
{
    Vector3 velocity_per_second = Vector3.zero;
    float time_until_random_change = 0.0f;

    public float time_between_random_changes = 5.0f;
    public float speed_per_second = 1.0f;

    public float angle_deg = 0.0f;
    public float random_angle_max = 10.0f;

    private void Start()
    {
        ResetAngle();
    }

    public void ResetAngle()
    {
        angle_deg = Random.Range(0, 360);
        velocity_per_second = new Vector3(Mathf.Cos(angle_deg * Mathf.Deg2Rad), Mathf.Sin(angle_deg * Mathf.Deg2Rad), 0.0f);
        velocity_per_second *= speed_per_second;
    }

    void Update()
    {
        time_until_random_change -= Time.deltaTime;            
        if(time_until_random_change <= 0.0f)
        {
            // Randomly change the direction, clamping the angle change.
            angle_deg = Random.Range(angle_deg - random_angle_max, angle_deg + random_angle_max);
            velocity_per_second = new Vector3(Mathf.Cos(angle_deg * Mathf.Deg2Rad), Mathf.Sin(angle_deg * Mathf.Deg2Rad), 0.0f);
            velocity_per_second *= speed_per_second;

            // Reset timer.
            time_until_random_change = time_between_random_changes;
        }

        transform.position += velocity_per_second * Time.deltaTime;

        if(transform.position.x < -5.0f) // LHS
        {
            transform.position = new Vector3(-5.0f, transform.position.y, 0.0f); // reset back into field.
            ResetAngle();
        }
        if (transform.position.x > 5.0f) // RHS
        {
            transform.position = new Vector3(5.0f, transform.position.y, 0.0f); // reset back into field.
            ResetAngle();
        }
        if (transform.position.y > 5.0f) // TOP
        {
            transform.position = new Vector3(transform.position.x, 5.0f, 0.0f); // reset back into field.
            ResetAngle();
        }
        if (transform.position.y < -5.0f) // BOT
        {
            transform.position = new Vector3(transform.position.x, -5.0f, 0.0f); // reset back into field.
            ResetAngle();
        }
    }
}
