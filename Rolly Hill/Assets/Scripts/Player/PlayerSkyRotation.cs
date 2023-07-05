using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkyRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 360;
    [SerializeField] int _axisIndex = 1;
    private float _rotationAmount;
    private Vector3 _rotation = new(0, 0, 0);

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _rotationAmount = Input.GetAxis("Mouse X");
            if (IsRotating())
            {
                _rotation[_axisIndex] = _rotationAmount * _rotationSpeed * Time.deltaTime;
                transform.Rotate(-_rotation);
            }
        }
    }

    bool IsRotating()
    {
        return _rotationAmount != 0;
    }
}
