using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _movementAmount = 0;
    [SerializeField] private float _horizontalSpeed;
    CharacterController _ch;
    private void Start()
    {
        _ch = GetComponent<CharacterController>();
    }
    void Update()
    {
        DetectScreenTouch();
    }

    void DetectScreenTouch()
    {
        if (Input.GetMouseButton(0))
        {
            _movementAmount = Input.GetAxis("Mouse X");
            if (_movementAmount != 0)
            {
                _ch.Move(_movementAmount * _horizontalSpeed * Time.fixedDeltaTime * Vector3.right);
            }
        }
    }
}
