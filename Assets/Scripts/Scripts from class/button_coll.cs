using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button_coll : MonoBehaviour
{
    public Button buttonA;
    public Button buttonB;

    // Update is called once per frame
    void Update()
    {
        bool is_on_lhs = buttonB.lhs >= buttonA.lhs || buttonB.rhs >= buttonA.lhs;

    }
}
