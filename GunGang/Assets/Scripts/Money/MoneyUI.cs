using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private RectTransformGettedRewardMovement _rewardCreator;
    [SerializeField] private Score _score;
    [SerializeField] private MoneyFromPlayerPrefs _money;
    int _totalMoneyImagesToCreate;

    public void CreateRewardImagesWithScore()
    {
        int score = _score.GetScore();
        _totalMoneyImagesToCreate = _money.ConvertScoreToMoney(score);
        _rewardCreator.CreateRewardObjects(_totalMoneyImagesToCreate);
    }
}
