using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyAdder : MonoBehaviour
{
    [SerializeField] private Score _score;
    [SerializeField] private MoneyFromPlayerPrefs _money;
    
    public void AddScoreAsMoney()
    {
        _money.AddScoreAsMoney(_score.GetScore());
    }
}
