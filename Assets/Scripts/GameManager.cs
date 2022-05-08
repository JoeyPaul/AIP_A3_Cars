using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // we need the AI's laps 
    public AiBehaviour ai1, ai2, ai3;
    int ai_1_laps = 1;
    int ai_2_laps = 1; // start on 1
    int ai_3_laps = 1;

    public VehicleController player;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ai_1_laps = ai1.laps;
        ai_2_laps = ai2.laps;
        ai_3_laps = ai3.laps;

        // Win checker
        if (player.laps == 3)
        {
            // player wins game hooray
        }
        else if (ai_1_laps == 3)
        {
            // Black car wins
        }
        else if (ai_2_laps == 3)
        {
            // Blue car wins
        }
        else if (ai_3_laps == 3)
        {
            // Green car wins
        }
    }
}
