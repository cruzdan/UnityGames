using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPoint : MonoBehaviour
{
    [SerializeField] private Vector2 _targetPoint;
    [SerializeField] private float _speed;
    [SerializeField] private float _errorDistance;
    private event Action OnReachTarget;
    private RectTransform _rt;
    private Vector3 _nextPosition;
    private void Start()
    {
        _rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        MoveToTarget();
        if (IsCloseToTarget())
        {
            enabled = false;
            OnReachTarget?.Invoke();
        }
    }

    void MoveToTarget()
    {
        CalculateNextPosition();
        _rt.position = _nextPosition;
    }

    void CalculateNextPosition()
    {
        _nextPosition = Vector2.Lerp(_rt.position, _targetPoint, _speed * Time.deltaTime);
        _nextPosition.z = _rt.position.z;
    }

    bool IsCloseToTarget()
    {
        return Vector2.Distance(_rt.position, _targetPoint) <= _errorDistance;
    }

    public void SubscribeToOnReachTarget(Action action)
    {
        OnReachTarget += action;
    }

    public void ClearOnReachTarget()
    {
        OnReachTarget = null;
    }

    public void SetTargetPoint(Vector2 target)
    {
        _targetPoint = target;
    }
}
