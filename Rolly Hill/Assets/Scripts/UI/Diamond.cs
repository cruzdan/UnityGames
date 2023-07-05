using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Diamond : MonoBehaviour
{
    [SerializeField] private int _totalDiamonds;
    [SerializeField] private Score _score;
    [SerializeField] private int _diamondCostOnScore;
    [SerializeField] private UIFollowPoint _diamondPrefab;
    [SerializeField] private Transform _diamondParent;
    [SerializeField] private RectTransform _cornerDiamondTarget;
    [SerializeField] private Vector2 _firstTargetMinLimit;
    [SerializeField] private Vector2 _firstTargetMaxLimit;
    [SerializeField] private TextMeshProUGUI _diamondNumberText;
    private List<UIFollowPoint> _diamonds = new();
    private void OnEnable()
    {
        PlayerSpeedDecrementer.OnPlayerStops += CreateDiamondsWithScore;
        CardMenu.OnRewardGetted += AddDiamonds;
        GameManager.OnPassLevel += SaveDiamonds;
    }

    private void OnDisable()
    {
        PlayerSpeedDecrementer.OnPlayerStops -= CreateDiamondsWithScore;
        CardMenu.OnRewardGetted -= AddDiamonds;
        GameManager.OnPassLevel -= SaveDiamonds;
    }

    private void Start()
    {
        InitDiamondsFromPlayerPrefs();
    }

    void CreateDiamondsWithScore()
    {
        int totalDiamondsToCreate = _score.GetScore() / _diamondCostOnScore;
        for (int i = 0; i < totalDiamondsToCreate; i++)
        {
            CreateDiamond();
            SetDiamondNearPointToFollow(i);
            AddDiamondCornerEvents(_diamonds[i]);
        }
    }

    void CreateDiamond()
    {
        _diamonds.Add(Instantiate(_diamondPrefab, _diamondParent));
    }

    void SetDiamondNearPointToFollow(int diamondIndex)
    {
        Vector2 nearTargetPoint = new Vector2(
            Random.Range(transform.position.x + _firstTargetMinLimit.x, transform.position.x + _firstTargetMaxLimit.x),
            Random.Range(transform.position.y + _firstTargetMinLimit.y, transform.position.y + _firstTargetMaxLimit.y)
            );
        _diamonds[diamondIndex].SetTargetPoint(nearTargetPoint);
    }

    void AddDiamondCornerEvents(UIFollowPoint diamondFollow)
    {
        diamondFollow.SubscribeToEvent(() => {
            SetDiamondFollowCornerPosition(diamondFollow);
            EnableDiamondFollow(diamondFollow);
            ClearDiamondEvent(diamondFollow);
            AddDiamondEventIncrementTotalDiamonds(diamondFollow);
            AddDiamondEventSetDiamondsText(diamondFollow);
            AddDiamondEventDestroyWhenReachTarget(diamondFollow);
        });
    }

    void SetDiamondFollowCornerPosition(UIFollowPoint diamondFollow)
    {
        diamondFollow.SetTargetPoint(_cornerDiamondTarget.position + Vector3.one * 12);
    }

    void EnableDiamondFollow(UIFollowPoint diamondFollow)
    {
        diamondFollow.enabled = true;
    }

    void ClearDiamondEvent(UIFollowPoint diamondFollow)
    {
        diamondFollow.ClearOnReachTarget();
    }

    void AddDiamondEventIncrementTotalDiamonds(UIFollowPoint diamondFollow)
    {
        diamondFollow.SubscribeToEvent(() => {
            _totalDiamonds++;
        });
    }

    void AddDiamondEventSetDiamondsText(UIFollowPoint diamondFollow)
    {
        diamondFollow.SubscribeToEvent(() => {
            _diamondNumberText.text = _totalDiamonds.ToString();
        });
    }

    void AddDiamondEventDestroyWhenReachTarget(UIFollowPoint diamondFollow)
    {
        diamondFollow.SubscribeToEvent(() => {
            Destroy(diamondFollow.gameObject);
            _diamonds.Remove(diamondFollow);
        });
    }

    public void AddDiamonds(int amount)
    {
        _totalDiamonds += amount;
        _diamondNumberText.text = _totalDiamonds.ToString();
    }

    public int GetTotalDiamonds()
    {
        return _totalDiamonds;
    }

    void InitDiamondsFromPlayerPrefs()
    {
        AddDiamonds(PlayerPrefs.GetInt("TotalDiamonds", 0));
    }

    public void SaveDiamonds()
    {
        PlayerPrefs.SetInt("TotalDiamonds", _totalDiamonds);
        PlayerPrefs.Save();
    }
}
