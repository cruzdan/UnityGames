using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int scorePlayer1 = 0;
    private int scorePlayer2 = 0;
    [SerializeField] private Text score1;
    [SerializeField] private Text score2;
    [SerializeField] private GameObject paddle1;
    [SerializeField] private GameObject paddle2;
    [SerializeField] private GameObject counter;

    public void IncrementScorePlayer1(int value) {
        scorePlayer1 += value;
        score1.text = scorePlayer1.ToString();
        IncrementScore();
    }

    public void IncrementScorePlayer2(int value) {
        scorePlayer2 += value;
        score2.text = scorePlayer2.ToString();
        IncrementScore();
    }

    void IncrementScore() {
        paddle1.GetComponent<PlayerMovement>().RestartPaddle();
        if (PlayerController.Instance.GetPlayers() == 1)
            paddle2.GetComponent<AIMovement>().RestartPaddle();
        else
            paddle2.GetComponent<PlayerMovement>().RestartPaddle();
        counter.GetComponent<Counter>().Reinitiate(); // typo
    }
}
