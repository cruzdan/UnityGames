using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongMovement : MonoBehaviour
{
    [Range(0, 2)] [SerializeField] private int _axisIndex;
    [SerializeField] private float _firstPosition;
    [SerializeField] private float _lastPosition;
    [SerializeField] private float _speed;
    Vector2 _movement;
    Transform _transform;
    bool _movingOnNegativeAxis;
    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        _movement = _transform.localPosition;
        if (_movingOnNegativeAxis)
        {
            MoveOnNegativeAxis();
            if (HasReachedFirstPosition())
            {
                GoOnPositiveAxis();
                _movement[_axisIndex] = _firstPosition;
            }
        }
        else
        {
            MoveOnPositiveAxis();
            if (HasReachedLastPosition())
            {
                GoOnNegativeAxis();
                _movement[_axisIndex] = _lastPosition;
            }
        }
        _transform.localPosition = _movement;
    }
    void MoveOnPositiveAxis()
    {
        _movement[_axisIndex] += _speed * Time.deltaTime;
    }
    bool HasReachedLastPosition()
    {
        return _movement[_axisIndex] >= _lastPosition;
    }
    void GoOnNegativeAxis()
    {
        _movingOnNegativeAxis = true;
    }
    void MoveOnNegativeAxis()
    {
        _movement[_axisIndex] -= _speed * Time.deltaTime;
    }
    bool HasReachedFirstPosition()
    {
        return _movement[_axisIndex] <= _firstPosition;
    }
    void GoOnPositiveAxis()
    {
        _movingOnNegativeAxis = false;
    }
}
