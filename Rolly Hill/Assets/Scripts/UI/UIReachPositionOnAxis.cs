using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIReachPositionOnAxis : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private int _axisIndex;
    [SerializeField] private float _targetPosition;
    [SerializeField] private float _errorDistance;
    [SerializeField] private UnityEvent OnTargetNear;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (IsNearOfTargetPosition())
        {
            OnTargetNear.Invoke();
            enabled = false;
        }
    }

    bool IsNearOfTargetPosition()
    {
        return Mathf.Abs(_rectTransform.anchoredPosition[_axisIndex] - _targetPosition) <= _errorDistance;
    }
}
