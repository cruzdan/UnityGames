using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitExplosion : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Vector3 _force;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddForce();
        }
    }
    public void AddForce()
    {
        _rb.AddForce(_force, ForceMode.Impulse);
    }
    public void SetUseGravity(bool value)
    {
        _rb.useGravity = value;
    }
    public void ResetRigidbodyVelocity()
    {
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
    }
}
