using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformPingPongOnAxis : MonoBehaviour
{
    [Range(0, 1)][SerializeField] private int _axisIndex;
    [SerializeField] private float _firstAnchoredPosition;
    [SerializeField] private float _lastAnchoredPosition;
    [SerializeField] private float _speed;
    private int _otherAxisIndex;
    Vector2 _movement;
    RectTransform _rectTransform;
    bool _movingOnNegativeAxis;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _otherAxisIndex = (_axisIndex + 1) % 2;
    }

    void Update()
    {
        if (_movingOnNegativeAxis)
        {
            MoveOnNegativeAxis();
            if (HasReachedFirstPosition())
            {
                GoOnPositiveAxis();
                _movement[_axisIndex]= _firstAnchoredPosition;
            }
        }
        else
        {
            MoveOnPositiveAxis();
            if (HasReachedLastPosition())
            {
                GoOnNegativeAxis();
                _movement[_axisIndex] = _lastAnchoredPosition;
            }
        }
        _movement[_otherAxisIndex] = _rectTransform.anchoredPosition[_otherAxisIndex];
        _rectTransform.anchoredPosition = _movement;
    }
    void MoveOnPositiveAxis()
    {
        _movement[_axisIndex] += _speed * Time.deltaTime;
    }
    bool HasReachedLastPosition()
    {
        return _movement[_axisIndex] >= _lastAnchoredPosition;
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
        return _movement[_axisIndex] <= _firstAnchoredPosition;
    }
    void GoOnPositiveAxis()
    {
        _movingOnNegativeAxis = false;
    }
}
