using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float _minimumScorePercentage;
    [SerializeField] private GameObject _getGiftButton;
    [SerializeField] private Score _score;
    [SerializeField] private Map _map;
    [SerializeField] private UIMobileElement _giftMobile;

    private void OnEnable()
    {
        Firework.OnFireworksEnd += TryEnableAndEnterGiftButton;
        GameManager.OnPassLevel += TryMoveOutGift;
    }

    private void OnDisable()
    {
        Firework.OnFireworksEnd -= TryEnableAndEnterGiftButton;
        GameManager.OnPassLevel -= TryMoveOutGift;
    }

    void TryMoveOutGift()
    {
        if (_getGiftButton.activeSelf)
        {
            _giftMobile.MoveOut();
        }
    }

    void TryEnableAndEnterGiftButton()
    {
        if (!HasGettedMinimumScoreForGift())
            return;
        _getGiftButton.SetActive(true);
        _giftMobile.MoveIn();
        
    }

    bool HasGettedMinimumScoreForGift()
    {
        return _score.GetScore() >= _map.GetTotalBlocks() * _minimumScorePercentage;
    }
}
