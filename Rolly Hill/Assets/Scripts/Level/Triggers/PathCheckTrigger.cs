using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCheckTrigger : MonoBehaviour
{
    public static event Action OnBallBigger;
    [SerializeField] private int _minimumBlocksToMakeBigger;
    private BlockCounter _ballBlockCounter;
    private Vector3 _increment = Vector3.one * 0.1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (BallCanBeBigger())
            {
                IncrementObjectScale(other.gameObject);
                OnBallBigger?.Invoke();
            }
            _ballBlockCounter.ResetBlockCounter();
        }
    }

    bool BallCanBeBigger()
    {
        return _ballBlockCounter.GetBlockCounter() >= _minimumBlocksToMakeBigger;
    }

    void IncrementObjectScale(GameObject objectToIncrementScale)
    {
        objectToIncrementScale.transform.localScale = objectToIncrementScale.transform.localScale + _increment;
    }

    public void SetBlockCounter(BlockCounter blockCounter)
    {
        _ballBlockCounter = blockCounter;
    }
}
