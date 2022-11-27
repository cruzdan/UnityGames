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
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private Text winText;
    [SerializeField] private int winScore;
    int totalPlayers;
    private void Start()
    {
        totalPlayers = PlayerManager.Instance.GetPlayers();
    }
    public void IncrementScorePlayer1(int value)
    {
        IncrementScore(ref scorePlayer1, ref score1, "Player 1 won", value);
    }
    public void IncrementScorePlayer2(int value)
    {
        if(totalPlayers == 1)
        {
            IncrementScore(ref scorePlayer2, ref score2, "Computer won", value);
        }
        else
        {
            IncrementScore(ref scorePlayer2, ref score2, "Player 2 won", value);
        }
    }
    public void IncrementScore(ref int score, ref Text scoreText, string winnerText, int value)
    {
        score += value;
        scoreText.text = score.ToString();
        if (score >= winScore)
        {
            pauseManager.SetPauseOnWin();
            GameOverMenu.SetActive(true);
            winText.text = winnerText;
        }
        else
        {
            IncrementScore();
        }
    }

    void IncrementScore()
    {
        paddle1.GetComponent<PlayerMovement>().RestartPaddle();
        if (PlayerManager.Instance.GetPlayers() == 1)
            paddle2.GetComponent<AIMovement>().RestartPaddle();
        else
            paddle2.GetComponent<PlayerMovement>().RestartPaddle();
        counter.GetComponent<Counter>().Reinitiate();
    }
}
