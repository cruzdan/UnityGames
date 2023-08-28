using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTarget : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _targetTransform;
    [SerializeField] private Rigidbody _rb;
    private Vector3 _auxiliarRotation = Vector3.zero;
    void Start()
    {
        _targetTransform = GameManager.Instance.GetPlayer().transform;
    }
    private void FixedUpdate()
    {
        SetYRotationToLookAtTarget();
        MoveRigidbodyPositionToTarget();
    }

    void SetYRotationToLookAtTarget()
    {
        transform.LookAt(_targetTransform);
        _auxiliarRotation.y = transform.eulerAngles.y;
        transform.eulerAngles = _auxiliarRotation;
    }

    void MoveRigidbodyPositionToTarget()
    {
        _rb.MovePosition(Vector3.MoveTowards(transform.position, _targetTransform.position, Time.fixedDeltaTime * _speed));
    }
}
