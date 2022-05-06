using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowsPlayer : MonoBehaviour
{
    public GameObject player;
    Vector3 cameraTransformPosition;

    // Update is called once per frame
    void Update()
    {
        cameraTransformPosition = player.transform.position;
        cameraTransformPosition.z = -10.0f;
        transform.position = cameraTransformPosition;
    }
}
