using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnPointUntilReachTarget : MonoBehaviour
{
    [SerializeField] private float _speed = 200;

    [SerializeField] private Transform _rotationPointTransform;
    [SerializeField] private Vector3 _rotationAxis = new Vector3(0, 0, 1);
    Vector3 _rotationPoint;

    [SerializeField] private float _targetZPosition;
    [SerializeField] private float _errorDistance;

    private float _distanceToTarget;
    private Transform _transform;
    private event Action OnTargetReached;

    void Start()
    {
        Init();
        _transform = transform;
    }

    public void Init()
    {
        SetRotationPointOfRotationTransform();
    }

    void SetRotationPointOfRotationTransform()
    {
        _rotationPoint = _rotationPointTransform.position;
    }

    void Update()
    {
        CalculateDistanceToTarget();
        DisableIfTargetHasReached();
        RotateAroundPoint();
    }

    void CalculateDistanceToTarget()
    {
        _distanceToTarget = _targetZPosition - _transform.rotation.z;
    }

    void DisableIfTargetHasReached()
    {
        if (HasReachedTarget())
        {
            OnTargetReached?.Invoke();
            enabled = false;
        }
    }

    bool HasReachedTarget()
    {
        return Mathf.Abs(_distanceToTarget) <= _errorDistance;
    }

    void RotateAroundPoint()
    {
        _transform.RotateAround(_rotationPoint, _rotationAxis, _speed * Time.deltaTime);
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
