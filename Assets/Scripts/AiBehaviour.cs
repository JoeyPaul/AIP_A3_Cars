using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBehaviour : MonoBehaviour
{
    [Header("Node Variables")]
    private Node[] allNodes; // A list of all nodes
    private Node currentNode = null; // The current selected node
    private Vector3 nodePos; // The current selected nodes position

    public int laps; // current laps

    [Header("Driving Variables")]

    private Rigidbody2D carRb; // The cars rigidbody which has all the forces applied to it
    [SerializeField] private float speedAccel; // The force applied to the car in the transform.right direction
    [SerializeField] private float SpeedTorque; // The torque applied on the car which dictates how quickly it can turn
    [SerializeField] private float minNodeDist; // How close to a node the car has to be before it moves to the selects the next node
    [SerializeField] private float breakBuffer; // A buffer that decreases the effectiveness of the break (the higher this is the less effect the break will have)

    private MapLoader mapLoader;
    private float speedAdjuster; // A force that changes based on what material the car is driving on

    [Header("Avoidance Variables")]
    private Vector3 left;
    private Vector3 right;
    private Vector3 avoidanceForcePerSec;
    private GameObject currentThreat;
     private bool isRight;
    [SerializeField] private float maxSight;
    [SerializeField] private float avoidanceSpeed;
    [SerializeField] private float worryRadius;

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
        Avoid();
        DetermineCollision();
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
        carRb.AddForce(((speedAccel * speedAdjuster * transform.right) + avoidanceForcePerSec) * Time.deltaTime);

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
    private bool DetermineCollision()
    {
        Vector3 velocityPerSec = carRb.velocity * Time.deltaTime;
        // This is where obstacles will be looked for and takes into account our current velocity.
        right = transform.position + (-transform.up * 0.5f) + velocityPerSec.normalized * maxSight;
        left = transform.position + (transform.up * 0.5f) + velocityPerSec.normalized * maxSight;
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("car");

        // If our ahead vector is within the radius of one of the obstacles then we have a threat
        float closestDistance = Mathf.Infinity;
        GameObject tempThreat = null;
        foreach (GameObject obstacle in obstacles)
        {
            if(obstacle != gameObject)
            {
                float distToCarRight = Vector3.Distance(obstacle.transform.position, right);
                float distToCarLeft = Vector3.Distance(obstacle.transform.position, left);
                if (distToCarRight <= closestDistance || distToCarLeft <= closestDistance)
                {
                    if(distToCarLeft < distToCarRight)
                    {
                        closestDistance = distToCarLeft;
                        isRight = false;
                    }
                    else
                    {
                        closestDistance = distToCarLeft;
                        isRight = true;
                    }
                    tempThreat = obstacle;
                }
            }
        }
        if (Vector3.Distance(tempThreat.transform.position, right) < worryRadius)
        {
            print("Threat found on deterministic level");
            currentThreat = tempThreat;
            return true;
        }
            currentThreat = null;
            return false;
    }
    private void Avoid()
    {
        avoidanceForcePerSec = Vector3.zero;

        // If there is a current threat then set the avoidance force to the required amount.
        if (currentThreat != null)
        {
            if(isRight)
                avoidanceForcePerSec = right - currentThreat.transform.position;
            else
                avoidanceForcePerSec = left - currentThreat.transform.position;
            avoidanceForcePerSec = avoidanceForcePerSec.normalized * avoidanceSpeed;
        }
        // If there is no required force then reset avoidance force.
        else
        {
            avoidanceForcePerSec = Vector3.zero;
        }
    }
    // Show a representation of my current velocity on the editor viewport.
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    if (carRb != null)
    //        Gizmos.DrawWireSphere(transform.position + (-transform.up * 0.5f) + (Vector3)carRb.velocity.normalized * maxSight, 0.5f);
    //    if (carRb != null)
    //        Gizmos.DrawWireSphere(transform.position + (transform.up * 0.5f) + (Vector3)carRb.velocity.normalized * maxSight, 0.5f);
    //}
}
