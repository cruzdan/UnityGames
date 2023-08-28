using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantLookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private Transform _transform;

    void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.LookAt(_target);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
}
