using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreResetter : MonoBehaviour
{
    [SerializeField] private Score _score;

    private void Start()
    {
        ResetScore();
    }

    void ResetScore()
    {
        _score.ResetScore();
    }
}
