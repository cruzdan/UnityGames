using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _forwardSpeed;
    private CharacterController _ch;
    private Vector3 _movement = new(0, 0, 0);

    #region Horizontal movement
    private float _movementAmount;
    [SerializeField] private float _horizontalSpeed;
    #endregion

    private void Start()
    {
        _ch = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _movement.x = 0;
        CalculateForwardDistance();
        CalculateHorizontalDistance();
        _ch.Move(_movement);
    }

    void CalculateForwardDistance()
    {
        _movement.z = _forwardSpeed * Time.fixedDeltaTime;
    }

    void CalculateHorizontalDistance()
    {
        if (IsPressingLeftClick())
        {
            _movementAmount = GetMouseMovementAmount();
            if (HasMoved())
            {
                SetNextDistanceOnXAxis();
            }
        }
    }

    bool IsPressingLeftClick()
    {
        return Input.GetMouseButton(0);
    }

    float GetMouseMovementAmount()
    {
        return Input.GetAxis("Mouse X");
    }

    bool HasMoved()
    {
        return _movementAmount != 0;
    }

    void SetNextDistanceOnXAxis()
    {
        _movement.x = _movementAmount * _horizontalSpeed * Time.fixedDeltaTime;
    }
}
