using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableBall : MonoBehaviour
{
    [SerializeField] private MoveOnAxisUntilReachTarget _rightMovement;
    [SerializeField] private MoveOnAxisUntilReachTarget _leftMovement;
    [SerializeField] private GlobalRotation _globalRotation;

    private void OnEnable()
    {
        _rightMovement.SubscribeToOnTargetReached(_globalRotation.SetContraryRotation);
        _leftMovement.SubscribeToOnTargetReached(_globalRotation.SetContraryRotation);
    }

    private void OnDisable()
    {
        _rightMovement.UnsubscribeToOnTargetReached(_globalRotation.SetContraryRotation);
        _leftMovement.UnsubscribeToOnTargetReached(_globalRotation.SetContraryRotation);
    }
}
