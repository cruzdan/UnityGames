using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationChanger : MonoBehaviour
{
    [SerializeField] private RotateOnPointUntilReachTarget _leftPendulumMovement;
    [SerializeField] private RotateOnPointUntilReachTarget _rightPendulumMovement;

    private void OnEnable()
    {
        _leftPendulumMovement.SubscribeToOnTargetReached(EnableRightPendulumMovement);
        _rightPendulumMovement.SubscribeToOnTargetReached(EnableLeftPendulumMovement);
    }

    private void OnDisable()
    {
        _leftPendulumMovement.UnsubscribeToOnTargetReached(EnableRightPendulumMovement);
        _rightPendulumMovement.UnsubscribeToOnTargetReached(EnableLeftPendulumMovement);
    }

    public void InitMovements()
    {
        _leftPendulumMovement.Init();
        _rightPendulumMovement.Init();
    }

    void EnableLeftPendulumMovement()
    {
        _leftPendulumMovement.enabled = true;
    }

    void EnableRightPendulumMovement()
    {
        _rightPendulumMovement.enabled = true;
    }
}
