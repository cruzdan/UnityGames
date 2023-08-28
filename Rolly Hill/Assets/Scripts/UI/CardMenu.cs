using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMenu : MonoBehaviour
{
    public static event Action<int> OnRewardGetted;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private Button _okButton;
    [SerializeField] private Transform _rewardCard;
    [SerializeField] private int _maxCardNumber;
    private int _rewardAmount;

    public void CardPressed()
    {
        SetCardsInteractable(false);
        EnableOkButton();
    }

    public void SetCardsInteractable(bool value)
    {
        int totalCards = _cardParent.childCount;
        for (int i = 0; i < totalCards; i++)
        {
            _cardParent.GetChild(i).GetComponent<Button>().interactable = value;
        }
    }

    void EnableOkButton()
    {
        _okButton.interactable = true;
    }

    public void SetRewardCardOnPosition(RectTransform newPositionTransform)
    {
        _rewardCard.position = newPositionTransform.position;
    }

    public void SetCardRandomNumber(Text cardText)
    {
        _rewardAmount = GetIntRandomNumber(1, _maxCardNumber + 1);
        cardText.text = _rewardAmount.ToString();
    }

    int GetIntRandomNumber(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public void RewardGetted()
    {
        OnRewardGetted?.Invoke(_rewardAmount);
    }
}
