using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput _detectKey;
    [SerializeField] private float _originalSpeed;
    [SerializeField] private MoveCharControllerOnDirection _moveForwardCharacter;
    [SerializeField] private PlayerSpeedDecrementer _playerSpeedDecrementer;

    private void OnEnable()
    {
        GoalCheck.OnGoalReached += DisableHorizontalMovement;
    }

    private void OnDisable()
    {
        GoalCheck.OnGoalReached -= DisableHorizontalMovement;
    }

    public void EnableHorizontalMovement()
    {
        _detectKey.enabled = true;
    }

    public void DisableHorizontalMovement()
    {
        _detectKey.enabled = false;
    }
    public void InitStopMovement()
    {
        _playerSpeedDecrementer.enabled = true;
    }
    public void ResetOriginalSpeed()
    {
        _moveForwardCharacter.SetSpeed(_originalSpeed);
    }

    public void EnableForwardMovement()
    {
        _moveForwardCharacter.enabled = true;
    }

    public void DisableForwardMovement()
    {
        _moveForwardCharacter.enabled = false;
    }
}