using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviour : MonoBehaviour
{
    private Node[] allNodes; // A list of all nodes
    private Node currentNode = null; // The current selected node
    private Vector3 nodePos; // The current selected nodes position


    private Rigidbody2D carRb; // The cars rigidbody which has all the forces applied to it
    [SerializeField] private float speedAccel; // The force applied to the car in the transform.right direction
    [SerializeField] private float SpeedTorque; // The torque applied on the car which dictates how quickly it can turn
    [SerializeField] private float minNodeDist; // How close to a node the car has to be before it moves to the selects the next node
    [SerializeField] private float breakBuffer; // A buffer that decreases the effectiveness of the break (the higher this is the less effect the break will have)

    [SerializeField] private MapLoader mapLoader;
    [SerializeField] private float speedAdjuster; // A force that changes based on what material the car is driving on

    private void Awake()
    {
        allNodes = FindObjectsOfType<Node>();
        carRb = GetComponent<Rigidbody2D>();
        mapLoader = FindObjectOfType<MapLoader>();
    }
    private void FixedUpdate()
    {
        FollowNode();
        AdjustSpeed();
    }

    void FollowNode()
    {
        // If there is no node selected, choose the closest one
        if (currentNode == null)
            currentNode = FindClosestNode();

        // If there is a node selected then set the target position to that node
        if (currentNode != null)
        {
            nodePos = currentNode.transform.position;
        }

        // If the AI is close enough to the selected node then the next node becomes the selected node
        float currentDist = Vector3.Distance(transform.position, nodePos);
        if (currentDist < minNodeDist)
            currentNode = currentNode.nextNode;

        // Determine the direction the car needs to be facing by using the cars facing direction and the currentNode
        Vector2 vecToNode = (nodePos - transform.position).normalized;
        float angleToNode = Vector2.SignedAngle(-transform.right, vecToNode) * -1;
        // Determine how the car needs to turn to face the currentNode
        float steerAmount = angleToNode / 45.0f;
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        // Apply the rotation
        carRb.AddTorque(steerAmount * SpeedTorque * Time.deltaTime);
        // Apply the acceleration
        carRb.AddForce((speedAccel * speedAdjuster) * transform.right * Time.deltaTime);

        // Calculate the angle from the car's facing direction to the currentNode facing angle - nextNode
        float nextNodeAngle = Vector2.Angle(transform.right, currentNode.transform.right - currentNode.nextNode.transform.right);
        // Apply the deceleration (this will increase/decrease depending on nextNodeAngle which makes the car slow down near corners)
        carRb.AddForce(((-nextNodeAngle * speedAdjuster) + breakBuffer) * transform.right * Time.deltaTime);
    }

    // Returns the nearest node to the AI
    Node FindClosestNode()
    {
        Node closestNode = allNodes[0];
        foreach (Node node in allNodes)
        {
            float currentDist = Vector3.Distance(transform.position, node.transform.position);
            float closestNodeDist = Vector3.Distance(transform.position, closestNode.transform.position);
            if (currentDist < closestNodeDist)
                closestNode = node;
        }
        return closestNode;
    }
    void AdjustSpeed()
    {
        MapLoader.Tile_Type type = mapLoader.GetSpeedForLocation(transform.position);

        switch (type)
        {
            case MapLoader.Tile_Type.ASPHALT:
                //rb.drag = 0.33f;
                speedAdjuster = mapLoader.tiles["ASPHALT"];
                break;
            case MapLoader.Tile_Type.MUD:
                //rb.drag = 1.0f;
                speedAdjuster = mapLoader.tiles["MUD"];
                break;
            case MapLoader.Tile_Type.DIRT:
                //rb.drag = 0.5fDIRT
                speedAdjuster = mapLoader.tiles["DIRT"];
                break;
            case MapLoader.Tile_Type.DEFAULT:
                //rb.drag = 0.33f;
                speedAdjuster = mapLoader.tiles["DEFAULT"];
                break;
        }
    }
}
