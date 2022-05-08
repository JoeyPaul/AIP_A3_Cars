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
    public GameObject gameStartCanvas;
    public GameObject gameplayCanvas;
    public GameObject endGameCanvas;
    public TMPro.TextMeshProUGUI lapsText;
    public TMPro.TextMeshProUGUI gameOverText;
    public string winner;

    // Update is called once per frame
    void Update()
    {
        ai_1_laps = ai1.laps;
        ai_2_laps = ai2.laps;
        ai_3_laps = ai3.laps;

        // Win checker
        if (player.laps == 3)
        {
            winner = "You win!";
            EndGame(winner);
        }
        else if (ai_1_laps == 3)
        {
            winner = "AI 1 Wins!";
            EndGame(winner);
        }
        else if (ai_2_laps == 3)
        {
            winner = "AI 2 Wins!";
            EndGame(winner);
        }
        else if (ai_3_laps == 3)
        {
            winner = "AI 3 Wins!";
            EndGame(winner);
        }

        lapsText.text = string.Format("Laps: {}", player.laps);
    }

    public void StartGame(float delayTime)
    {
        StartCoroutine(DelayStart(delayTime));
    }

    IEnumerator DelayStart(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        gameStartCanvas.SetActive(false);
        gameplayCanvas.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void EndGame(string winner)
    {
        gameplayCanvas.SetActive(false);
        endGameCanvas.SetActive(true);
        gameOverText.text = winner;
    }
}