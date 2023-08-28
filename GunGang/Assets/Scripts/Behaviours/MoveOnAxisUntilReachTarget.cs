using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnAxisUntilReachTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _errorDistance;
    [SerializeField] private Vector3 _movementAxis;
    [SerializeField] private Transform _targetTransform;
    private Vector3 _targetPosition;
    private event Action OnTargetReached;
    Transform _transform;
    private void Start()
    {
        _transform = transform;
    }

    public void Init()
    {
        _targetPosition = _targetTransform.position;
    }

    void Update()
    {
        MoveOnAxis();
        if (HasReachedTarget())
        {
            enabled = false;
            OnTargetReached?.Invoke();
        }
    }

    void MoveOnAxis()
    {
        _transform.position += Time.deltaTime * _speed * _movementAxis;
    }

    bool HasReachedTarget()
    {
        return Vector3.Distance(_transform.position, _targetPosition) <= _errorDistance;
    }

    public void SubscribeToOnTargetReached(Action action)
    {
        OnTargetReached += action;
    }

    public void UnsubscribeToOnTargetReached(Action action)
    {
        OnTargetReached -= action;
    }
}