using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] scoreDigitsSR;
    [SerializeField] private SpriteRenderer[] highScoreDigitsSR;
    [SerializeField] private GameManager gameManager;
    int currentScore = 0;
    int highScore = 0;
    int activeScoreDigits = 2;
    int activeHighScoreDigits = 0;
    //auxiliar value to active a digit when it is needed
    int k = 100;
    //auxiliar value to update digits form score and high score
    int L;
    [SerializeField] private Sprite[] numbers;

    //1 extra life score
    readonly int score1Life = 10000;
    bool lifeGetted = false;
    void ActiveNextScoreDigit()
    {
        scoreDigitsSR[activeScoreDigits].gameObject.SetActive(true);
        activeScoreDigits++;
    }
    void ActiveNextHighScoreDigit()
    {
        highScoreDigitsSR[activeHighScoreDigits].gameObject.SetActive(true);
        activeHighScoreDigits++;
    }
    public void AddScore(int value)
    {
        if(!lifeGetted && currentScore >= score1Life)
        {
            gameManager.AddLife();
            lifeGetted = true;
        }
        currentScore += value;
        if(currentScore >= k)
        {
            k *= 10;
            ActiveNextScoreDigit();
        }
        if(activeHighScoreDigits < activeScoreDigits)
        {
            while(activeHighScoreDigits < activeScoreDigits)
            {
                ActiveNextHighScoreDigit();
            }
        }
        if(highScore < currentScore)
        {
            highScore = currentScore;
            UpdateDigits(highScoreDigitsSR, highScore, activeHighScoreDigits);
        }
        UpdateDigits(scoreDigitsSR, currentScore, activeScoreDigits);
    }
    private void UpdateDigits(SpriteRenderer[] digits, int newValue, int total)
    {
        L = 10;
        for (int i = 1; i < total; i++)
        {
            int number = newValue % (L * 10);
            int searchedNumber = (int)(number / L);
            if (!digits[i].sprite.Equals(numbers[searchedNumber]))
            {
                digits[i].sprite = numbers[searchedNumber];
            }
            L *= 10;
        }
    }
}
