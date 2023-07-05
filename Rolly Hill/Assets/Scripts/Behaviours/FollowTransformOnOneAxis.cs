using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformOnOneAxis : MonoBehaviour
{
    //0 -> X Axis, 1-> Y Axis, 2 -> Z Axis
    [Range(0,2)][SerializeField] private int _axisIndex;
    [SerializeField] private Transform _followingTransform;
    private Transform _thisTransform;
    private float _distanceToTarget;
    private Vector3 _currentPosition = new();
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _thisTransform = transform;
        AnchorDistanceToTarget();
    }

    public void SetFollowingTransform(Transform followingTransform)
    {
        _followingTransform = followingTransform;
    }

    public void SetAxisFollowing(int index)
    {
        _axisIndex = index;
    }

    void AnchorDistanceToTarget()
    {
        _distanceToTarget = transform.position[_axisIndex] - _followingTransform.position[_axisIndex];
    }

    private void LateUpdate()
    {
        AlignPositionWithTargetAxis();
    }

    void AlignPositionWithTargetAxis()
    {
        _currentPosition = _thisTransform.position;
        _currentPosition[_axisIndex] = _followingTransform.position[_axisIndex] + _distanceToTarget;
        _thisTransform.position = _currentPosition;
    }
}
