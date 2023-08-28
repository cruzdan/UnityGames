using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollisions : MonoBehaviour
{
    [SerializeField] private GameEvent OnBlockTouched;
    [SerializeField] private BoxCollider _boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ParentToTransform(other.transform);
            DisableCollider();
            OnBlockTouched.TriggerEvent();
        }
    }
    void ParentToTransform(Transform parentTransform)
    {
        transform.SetParent(parentTransform);
    }
    void DisableCollider()
    {
        _boxCollider.enabled = false;
    }
}
