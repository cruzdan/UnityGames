using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationSpeed;
    [SerializeField] private float _initialRotationX;
    void Update()
    {
        transform.Rotate(_rotationSpeed, Space.World);
    }
    public void DecrementRotationSpeedX(float amount)
    {
        _rotationSpeed.x -= amount;
    }
    public Vector3 GetRotationSpeed()
    {
        return _rotationSpeed;
    }
    public float GetRotationSpeedX()
    {
        return _rotationSpeed.x;
    }
    public void SetRotationSpeedX(float value)
    {
        _rotationSpeed.x = value;
    }
    public void ResetRotationX()
    {
        _rotationSpeed.x = _initialRotationX;
    }
}
