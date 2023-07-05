using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedDecrementer : MonoBehaviour
{
    [SerializeField] private float _decrementSpeed;
    [SerializeField] private MoveCharControllerOnDirection _moveForwardCharacter;

    public static event Action<float> OnFireworksAppear;
    public static event Action OnPlayerStops;
    void FixedUpdate()
    {
        DecrementForwardSpeed();
        if (HasNegativeForwardVelocity())
        {
            OnFireworksAppear?.Invoke(transform.position.z);
            OnPlayerStops?.Invoke();
            DisableForwardMovement();
            enabled = false;
        }
    }

    void DecrementForwardSpeed()
    {
        _moveForwardCharacter.DecrementSpeed(_decrementSpeed * Time.fixedDeltaTime);
    }

    bool HasNegativeForwardVelocity()
    {
        return _moveForwardCharacter.GetSpeed() <= 0;
    }

    public void DisableForwardMovement()
    {
        _moveForwardCharacter.enabled = false;
    }
}
