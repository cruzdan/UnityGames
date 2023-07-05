using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyPhase : MonoBehaviour
{
    public static event Action OnSkyPhase;
    public static event Action OnFloorPhase;
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _fallSpeed = 20;
    [SerializeField] private float _upPosition = 30;
    [SerializeField] private float _downPosition = 1;
    bool _goingUp;
    Vector3 _position;
    public void InitGoingUp()
    {
        _goingUp = true;
        SetCurrentPosition();
    }
    public void InitGoingDown()
    {
        _goingUp = false;
        SetCurrentPosition();
    }
    private void Update()
    {
        if (_goingUp)
        {
            MovePositionUp();
            if(HasReachedUpPosition())
            {
                GameManager.Instance.SetOnSkyPhase();
                OnSkyPhase?.Invoke();
            }
        }
        else
        {
            MovePositionDown();
            if (HasReachedDownPosition())
            {
                GameManager.Instance.SetOnFloorPhase();
                OnFloorPhase?.Invoke();
            }
        }
    }

    void MovePositionUp()
    {
        _position.y += _speed * Time.deltaTime;
        transform.position = _position;
    }

    bool HasReachedUpPosition()
    {
        return _position.y >= _upPosition;
    }

    void MovePositionDown()
    {
        _position.y -= _fallSpeed * Time.deltaTime;
        transform.position = _position;
    }

    bool HasReachedDownPosition()
    {
        return _position.y <= _downPosition;
    }

    void SetCurrentPosition()
    {
        _position = transform.position;
    }
}
