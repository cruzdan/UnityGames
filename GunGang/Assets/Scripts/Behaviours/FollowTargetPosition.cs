using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTargetPosition : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private float _speed;
    [SerializeField] private float _errorDistance;
    private event Action OnTargetReached;
    private Vector3 _auxiliarRotation = Vector3.zero;
    private Transform _transform;
    private void OnEnable()
    {
        _transform = transform;
    }

    public void SetTarget(Vector3 target)
    {
        _targetPosition = target;
    }

    private void FixedUpdate()
    {
        SetYRotationToLookAtTarget();
        MovePositionToTarget();
        DisableIfReachTarget();
    }

    void SetYRotationToLookAtTarget()
    {
        _transform.LookAt(_targetPosition);
        _auxiliarRotation.y = _transform.eulerAngles.y;
        _transform.eulerAngles = _auxiliarRotation;
    }

    void MovePositionToTarget()
    {
        _transform.position = Vector3.MoveTowards(_transform.position, _targetPosition, Time.fixedDeltaTime * _speed);
    }

    void DisableIfReachTarget()
    {
        if(Vector3.Distance(_transform.position, _targetPosition) <= _errorDistance)
        {
            enabled = false;
            OnTargetReached?.Invoke();
        }
    }

    public void SubscribeToOnTargetReached(Action action)
    {
        OnTargetReached += action;
    }

    public void UnsubscribeToOnTargetReached(Action action)
    {
        OnTargetReached -= action;
    }

    public void ClearOnTargetReached()
    {
        OnTargetReached = null;
    }
}