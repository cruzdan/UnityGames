using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDrop : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private Rigidbody _rigidbody;
    public void DropFromPositionWithForce(Vector3 initialPosition, Vector3 force)
    {
        SetEndBlockLayer();
        transform.parent = null;
        transform.position = initialPosition;
        UndockCollider();
        ActivateRigidbodyForces(force);
    }
    void UndockCollider()
    {
        _boxCollider.isTrigger = false;
        _boxCollider.enabled = true;
    }
    void ActivateRigidbodyForces(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _rigidbody.useGravity = true;
    }
    void SetEndBlockLayer()
    {
        gameObject.layer = 6;
    }
    public void ResetLayer()
    {
        gameObject.layer = 0;
    }
    public void RestartDropValues()
    {
        _boxCollider.isTrigger = true;
        ResetRigidbodyVelocity();
    }
    void ResetRigidbodyVelocity()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
