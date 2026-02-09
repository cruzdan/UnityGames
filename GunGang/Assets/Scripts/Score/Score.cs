using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Score")]
public class Score : ScriptableObject
{
    public static Action<int> OnScoreChanged;
    [SerializeField] private int _score = 0;

    public void IncrementScore(int value)
    {
        _score += value;
        OnScoreChanged?.Invoke(_score);
        GameManager.Instance.StartArrayBounce(GameManager.Instance.ScoreBounces);
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
