using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkBlock : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _forceAmount;
    [SerializeField] private float _maxTimeToDisable;
    private float _timeElapsed;
    public void Init(Vector3 position, Vector3 forceDirection)
    {
        transform.SetPositionAndRotation(position, Quaternion.identity);
        AssignRigidbodyForce(forceDirection);
        ResetTimeElapsed();
    }

    void AssignRigidbodyForce(Vector3 forceDirection)
    {

        _rb.useGravity = true;
        _rb.velocity = _forceAmount * forceDirection;
    }

    void ResetTimeElapsed()
    {
        _timeElapsed = 0;
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (HasElapsedMaxTime())
        {
            Disable();
        }
    }

    bool HasElapsedMaxTime()
    {
        return _timeElapsed >= _maxTimeToDisable;
    }

    void Disable()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
}
