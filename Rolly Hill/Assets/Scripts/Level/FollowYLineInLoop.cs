using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowYLineInLoop : MonoBehaviour
{
    [SerializeField] private float _upperPosition;
    [SerializeField] private float _bottomPosition;
    [SerializeField] private float _speed;
    Vector3 _position;
    void Start()
    {
        _position = transform.position;
    }
    void Update()
    {
        UpperPosition();
        if (HasReachedUpperPosition())
        {
            _position.y = _bottomPosition;
        }
        transform.position = _position;
    }
    void UpperPosition()
    {
        _position.y += _speed * Time.deltaTime;
    }
    bool HasReachedUpperPosition()
    {
        return _position.y >= _upperPosition;
    }
}
