using System;
using UnityEngine;

public class PlayerSpeedDecrementer : MonoBehaviour
{
    [SerializeField] private float _decrementSpeed;
    [SerializeField] private MoveCharControllerOnDirection _moveForwardCharacter;
    [SerializeField] private GameEvent OnPlayerStops;

    public static event Action<float> OnFireworksAppear;
    void FixedUpdate()
    {
        DecrementForwardSpeed();
        if (HasNegativeForwardVelocity())
        {
            OnFireworksAppear?.Invoke(transform.position.z);
            OnPlayerStops.TriggerEvent();
            CameraShake.Instance.Shake(0.1f, 0.2f);
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
