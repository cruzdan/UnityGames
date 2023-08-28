using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyFromPlayerPrefs : MonoBehaviour
{
    [SerializeField] private int _moneyCostOnScore;
    [SerializeField] private int _money;
    public static Action<int> OnMoneyChange;

    private void Awake()
    {
        InitMoneyFromPlayerPrefs();
    }
    public void AddMoney(int amount)
    {
        _money += amount;
        OnMoneyChange?.Invoke(_money);
    }

    public void AddScoreAsMoney(int score)
    {
        int money = ConvertScoreToMoney(score);
        AddMoney(money);
    }

    void InitMoneyFromPlayerPrefs()
    {
        AddMoney(PlayerPrefs.GetInt("Money", 0));
    }

    public void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", _money);
        PlayerPrefs.Save();
    }

    public int ConvertScoreToMoney(int score)
    {
        return score / _moneyCostOnScore;
    }

    public int GetMoney()
    {
        return _money;
    }
}
