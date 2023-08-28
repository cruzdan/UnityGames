using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToForward : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    Transform _transform;
    Vector3 _auxiliarRotation = Vector3.zero;
    private event Action OnCompletedRotation;

    private void Start()
    {
        _transform = transform;
    }
    void Update()
    {
        if (AngleIsPositive())
        {
            DecrementYAuxiliarRotation();
            if (AuxiliarYRotationIsOutOfLimits())
            {
                ResetAuxiliarYRotation();
            }
        }
        else if (AngleIsNegative())
        {
            IncrementYAuxiliarRotation();
            if (AuxiliarYRotationIsWithinOfLimits())
            {
                ResetAuxiliarYRotation();
            }
        }
        else
        {
            enabled = false;
            OnCompletedRotation?.Invoke();
            return;
        }
        _transform.eulerAngles = _auxiliarRotation;
    }

    bool AngleIsPositive()
    {
        return _transform.eulerAngles.y > 0;
    }

    void DecrementYAuxiliarRotation()
    {
        _auxiliarRotation.y = -_rotationSpeed * Time.deltaTime + _transform.eulerAngles.y;
    }

    bool AuxiliarYRotationIsOutOfLimits()
    {
        return _auxiliarRotation.y <= 0 || _auxiliarRotation.y >= 345;
    }

    void ResetAuxiliarYRotation()
    {
        _auxiliarRotation.y = 0;
    }

    bool AngleIsNegative()
    {
        return _transform.eulerAngles.y < 0;
    }

    void IncrementYAuxiliarRotation()
    {
        _auxiliarRotation.y = _rotationSpeed * Time.deltaTime + _transform.eulerAngles.y;
    }

    bool AuxiliarYRotationIsWithinOfLimits()
    {
        return _auxiliarRotation.y >= 0;
    }

    public void ResetVariables()
    {
        _auxiliarRotation = Vector3.zero;
    }

    public void SubscribeToOnCompletedRotation(Action method)
    {
        OnCompletedRotation += method;
    }

    public void UnsubscribeToOnCompletedRotation(Action method)
    {
        OnCompletedRotation -= method;
    }

    public void ClearOnCompletedRotation()
    {
        OnCompletedRotation = null;
    }
}
