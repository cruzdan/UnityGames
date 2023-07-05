using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharControllerOnDirection : MonoBehaviour
{
    //0 -> X Axis, 1-> Y Axis, 2 -> Z Axis
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _speed;
    CharacterController _ch;

    private void Start()
    {
        _ch = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveOnDirection();
    }

    void MoveOnDirection()
    {
        _ch.Move(Time.fixedDeltaTime * _speed * _direction);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void DecrementSpeed(float value)
    {
        _speed -= value;
    }

    public float GetSpeed()
    {
        return _speed;
    }
}
