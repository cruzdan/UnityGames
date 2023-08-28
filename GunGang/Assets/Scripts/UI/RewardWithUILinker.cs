using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardWithUILinker : MonoBehaviour
{
    [SerializeField] private RectTransformGettedRewardMovement _rewardCreator;
    [SerializeField] private MoneyFromPlayerPrefs _money;
    [SerializeField] private TextMeshProUGUI _rewardText;
    List<UIFollowPoint> _rewards;

    public void AddUpdateUITextEventWhenReachTargetToAllRewards()
    {
        _rewards = _rewardCreator.GetRewards();
        foreach(var reward in _rewards)
        {
            AddRewardUpdateUIEvent(reward);
        }
        _rewards = null;
    }

    void AddRewardUpdateUIEvent(UIFollowPoint reward)
    {
        reward.SubscribeToOnReachTarget(() => {
            AddRewardEventIncrementAndUpdateUIText(reward);
        });
    }

    void AddRewardEventIncrementAndUpdateUIText(UIFollowPoint reward)
    {
        reward.SubscribeToOnReachTarget(() =>
        {
            IncrementAndUpdateUIText();
        });
    }

    void IncrementAndUpdateUIText()
    {
        int newUIValue = int.Parse(_rewardText.text) + 1;
        _rewardText.text = newUIValue.ToString();
    }
}
