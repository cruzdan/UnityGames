using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillGiftWithScore : MonoBehaviour
{
    [SerializeField] private Score _score;
    [SerializeField] private FillImageWithTime _fillImageWithTime;
    [SerializeField] private Map _map;
    [SerializeField] private UIReachPositionOnAxis _rewardFillReachPisition;

    private void OnEnable()
    {
        Firework.OnFireworksEnd += Init;
        Firework.OnFireworksEnd += ActivateRewardFillWhenReachCorrectPosition;
    }

    private void OnDisable()
    {
        Firework.OnFireworksEnd -= Init;
        Firework.OnFireworksEnd -= ActivateRewardFillWhenReachCorrectPosition;
    }

    void Init()
    {
        float num = (float)_score.GetScore() / (float)_map.GetTotalBlocks();
        _fillImageWithTime.SetMaxPercentage(num);
        _fillImageWithTime.Init();
    }

    void ActivateRewardFillWhenReachCorrectPosition()
    {
        _rewardFillReachPisition.enabled = true;
    }
}
