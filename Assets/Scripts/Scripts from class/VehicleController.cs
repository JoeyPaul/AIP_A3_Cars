using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float torque;
    private float speedScalar = 0.0f;

    public int laps = 1;

    //public FlowFieldManager ffm;
    public MapLoader ml;
    // Checkpoints
    public List<Checkpoint> checkpoints = new List<Checkpoint>();

    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.right * (speed * speedScalar) * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.right * (speed * speedScalar) * Time.deltaTime);
        }

        #region Rotation
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(torque * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(-torque * Time.deltaTime);
        }
        #endregion


        MapLoader.Tile_Type type = ml.GetSpeedForLocation(transform.position);


        switch (type)
        {
            case MapLoader.Tile_Type.ASPHALT:
                //rb.drag = 0.33f;
                speedScalar = ml.tiles["ASPHALT"];
                break;
            case MapLoader.Tile_Type.MUD:
                //rb.drag = 1.0f;
                speedScalar = ml.tiles["MUD"];
                break;
            case MapLoader.Tile_Type.DIRT:
                //rb.drag = 0.5f;
                speedScalar = ml.tiles["DIRT"];
                break;
            case MapLoader.Tile_Type.DEFAULT:
                //rb.drag = 0.33f;
                speedScalar = ml.tiles["DEFAULT"];
                break;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // hit start
        if (col.gameObject == checkpoints[0].gameObject)
        {
            Debug.Log("hit start");
            checkpoints[0].active = true;
            // lap condition.
            if (checkpoints[1].active && checkpoints[2].active && checkpoints[3].active)
            {
                laps++;
                ResetCheckPoints();
            }
        }
        else if (col.gameObject == checkpoints[1].gameObject)
        {
            Debug.Log("hit check 1");
            checkpoints[1].active = true;
        }
        else if (col.gameObject == checkpoints[2].gameObject)
        {
            Debug.Log("hit check 2");
            checkpoints[2].active = true;
        }
        else 
        {
            Debug.Log("hit check 3");
            checkpoints[3].active = true;
        }
    }
    void ResetCheckPoints()
    {
        checkpoints[1].active = false;
        checkpoints[2].active = false;
        checkpoints[3].active = false;
    }
}
