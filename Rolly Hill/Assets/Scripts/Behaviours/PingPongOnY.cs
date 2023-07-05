using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PingPongOnY : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _upperLimit;
    [SerializeField] private float _bottomLimit;
    private bool goingUp = true;
    private Vector3 _movement;
    void Start()
    {
        InitPosition();
    }
    public void InitPosition()
    {
        _movement = transform.position;
    }
    void Update()
    {
        if (goingUp)
        {
            MoveUp();
            if (HasReachedUpperLimit())
            {
                GoDown();
                _movement.y = _upperLimit;
            }
        }
        else
        {
            MoveDown();
            if (HasReachedLowerLimit())
            {
                GoUp();
                _movement.y = _bottomLimit;
            }
        }
        transform.position = _movement;
    }
    void MoveUp()
    {
        _movement.y += _speed * Time.deltaTime;
    }
    bool HasReachedUpperLimit()
    {
        return _movement.y >= _upperLimit;
    }
    void GoDown()
    {
        goingUp = false;
    }
    void MoveDown()
    {
        _movement.y -= _speed * Time.deltaTime;
    }
    bool HasReachedLowerLimit()
    {
        return _movement.y <= _bottomLimit;
    }
    void GoUp()
    {
        goingUp = true;
    }
}
