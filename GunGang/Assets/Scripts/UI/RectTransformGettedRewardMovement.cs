using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformGettedRewardMovement : MonoBehaviour
{
    [SerializeField] private Vector2 _firstTargetMinLimit;
    [SerializeField] private Vector2 _firstTargetMaxLimit;
    [SerializeField] private RectTransform _secondTarget;
    [SerializeField] private UIFollowPoint _rewardPrefab;
    [SerializeField] private Transform _prefabParent;
    private List<UIFollowPoint> _rewardObjects = new();

    public void CreteMandanga()
    {
        CreateRewardObjects(5);
    }

    public void CreateRewardObjects(int totalRewardObjectsToCreate)
    {
        for (int i = 0; i < totalRewardObjectsToCreate; i++)
        {
            CreateRewardObject();
            SetRewardObjectNearPointToFollow(i);
            AddRewardObjectCornerEvents(_rewardObjects[i]);
        }
    }

    void CreateRewardObject()
    {
        _rewardObjects.Add(Instantiate(_rewardPrefab, _prefabParent));
    }

    void SetRewardObjectNearPointToFollow(int rewardIndex)
    {
        Vector2 nearTargetPoint = new Vector2(
            Random.Range(transform.position.x + _firstTargetMinLimit.x, transform.position.x + _firstTargetMaxLimit.x),
            Random.Range(transform.position.y + _firstTargetMinLimit.y, transform.position.y + _firstTargetMaxLimit.y)
            );
        _rewardObjects[rewardIndex].SetTargetPoint(nearTargetPoint);
    }

    void AddRewardObjectCornerEvents(UIFollowPoint reward)
    {
        reward.SubscribeToOnReachTarget(() => {
            SetRewardFollowCornerPosition(reward);
            EnableRewardFollow(reward);
            ClearRewardEvent(reward);
            AddRewardEventDestroyWhenReachTarget(reward);
        });
    }

    void SetRewardFollowCornerPosition(UIFollowPoint reward)
    {
        reward.SetTargetPoint(_secondTarget.position);
    }

    void EnableRewardFollow(UIFollowPoint reward)
    {
        reward.enabled = true;
    }

    void ClearRewardEvent(UIFollowPoint reward)
    {
        reward.ClearOnReachTarget();
    }

    void AddRewardEventDestroyWhenReachTarget(UIFollowPoint reward)
    {
        reward.SubscribeToOnReachTarget(() => {
            DestroyAndRemoveReward(reward);
        });
    }

    void DestroyAndRemoveReward(UIFollowPoint reward)
    {
        Destroy(reward.gameObject);
        _rewardObjects.Remove(reward);
    }

    public List<UIFollowPoint> GetRewards()
    {
        return _rewardObjects;
    }

    public void DeleteAllRewardObjects()
    {
        int total = _rewardObjects.Count;
        for (int i = total - 1; i >= 0; i--)
        {
            DestroyAndRemoveReward(_rewardObjects[i]);
        }
    }
}
