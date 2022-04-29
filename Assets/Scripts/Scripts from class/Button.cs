using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public float lhs;
    public float rhs;
    public float bottom;
    public float top;

    // Start is called before the first frame update
    void Update()
    {
        lhs = transform.position.x - (transform.localScale.x / 2);
        rhs = transform.position.x + (transform.localScale.x / 2);
        bottom = transform.position.y - (transform.localScale.y / 2);
        top = transform.position.y + (transform.localScale.y / 2);
    }

}
