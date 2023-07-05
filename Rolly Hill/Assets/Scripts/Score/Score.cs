using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static event Action<int> OnScoreChanged;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _incrementScoreAmount = 1;

    private void OnEnable()
    {
        BlockCollisions.OnPlayerTouched += IncrementScore;
        GameManager.OnPassLevel += ResetScore;
        LoosingPhase.OnLoose += ResetScore;
    }

    private void OnDisable()
    {
        BlockCollisions.OnPlayerTouched -= IncrementScore;
        GameManager.OnPassLevel -= ResetScore;
        LoosingPhase.OnLoose -= ResetScore;
    }

    public void IncrementScore()
    {
        _score += _incrementScoreAmount;
        OnScoreChanged?.Invoke(_score);
    }

    public void ResetScore()
    {
        _score = 0;
        OnScoreChanged?.Invoke(_score);
    }

    public int GetScore()
    {
        return _score;
    }
}
