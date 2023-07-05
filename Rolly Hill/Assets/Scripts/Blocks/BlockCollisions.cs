using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCollisions : MonoBehaviour
{
    [SerializeField] public static event Action OnPlayerTouched;
    [SerializeField] private BoxCollider _boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ParentToTransform(other.transform);
            DisableCollider();
            OnPlayerTouched?.Invoke();
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
