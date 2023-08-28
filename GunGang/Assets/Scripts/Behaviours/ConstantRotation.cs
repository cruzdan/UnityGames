using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _rotationVector;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.Rotate(Time.deltaTime * _speed * _rotationVector);
    }

    public void SetRotationVector(float x, float y, float z)
    {
        _rotationVector.x = x;
        _rotationVector.y = y;
        _rotationVector.z = z;
    }
}
