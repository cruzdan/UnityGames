using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRectTransformOnAxis : MonoBehaviour
{
    //0-> x, 1-> y
    [Range(0, 1)] [SerializeField] private int _axisIndex;
    [SerializeField] private float _finalAnchoredPosition;
    [SerializeField] private bool _disableWhenReachFinalPosition;
    [SerializeField] private float _errorDistance = 20;
    [SerializeField] private float _speed = 1;

    private RectTransform _currentTransform;
    private Vector2 _targetPosition;

    void Start()
    {
        _currentTransform = GetComponent<RectTransform>();
        _targetPosition = _currentTransform.anchoredPosition;
        _targetPosition[_axisIndex] = _finalAnchoredPosition;
    }

    void Update()
    {
        UpdatePosition();
        if (IsCloseOfFinalPosition())
        {
            Disable();
        }
    }

    void UpdatePosition()
    {
        _currentTransform.anchoredPosition = Vector2.Lerp(_currentTransform.anchoredPosition, _targetPosition, _speed * Time.deltaTime);
    }

    bool IsCloseOfFinalPosition()
    {
        return Mathf.Abs(_currentTransform.anchoredPosition[_axisIndex] - _finalAnchoredPosition) <= _errorDistance;
    }

    void Disable()
    {
        enabled = false;
        if (_disableWhenReachFinalPosition)
        {
            gameObject.SetActive(false);
        }
    }
}
