using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnAxisChanger : MonoBehaviour
{
    [SerializeField] private MoveOnAxisUntilReachTarget _firstMovement;
    [SerializeField] private MoveOnAxisUntilReachTarget _secondMovement;

    private void OnEnable()
    {
        _firstMovement.SubscribeToOnTargetReached(EnableRightCarMovement);
        _secondMovement.SubscribeToOnTargetReached(EnableLeftCarMovement);
    }

    private void OnDisable()
    {
        _firstMovement.UnsubscribeToOnTargetReached(EnableRightCarMovement);
        _secondMovement.UnsubscribeToOnTargetReached(EnableLeftCarMovement);
    }

    void EnableLeftCarMovement()
    {
        _firstMovement.enabled = true;
    }

    void EnableRightCarMovement()
    {
        _secondMovement.enabled = true;
    }

    public void InitMovements()
    {
        _firstMovement.Init();
        _secondMovement.Init();
    }
}
