using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotationSpeed;
    private Transform _transform;
    private void Start()
    {
        _transform = transform;
    }
    void Update()
    {
        _transform.Rotate(_rotationSpeed * Time.deltaTime, Space.World);
    }
    public void DecrementRotationSpeedX(float amount)
    {
        _rotationSpeed.x -= amount;
    }
    public Vector3 GetRotationSpeed()
    {
        return _rotationSpeed;
    }

    public void SetContraryRotation()
    {
        _rotationSpeed *= -1;
    }
}
