using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ConstantRotationPoint : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector3 _rotationAxis;

    [SerializeField] private Transform _rotationPointTransform;
    Vector3 _rotationPoint;
    Transform _transform;

    void Start()
    {
        _transform = transform;
        Init();
    }

    public void Init()
    {
        SetRotationPointOfRotationTransform();
    }

    void SetRotationPointOfRotationTransform()
    {
        _rotationPoint = _rotationPointTransform.position;
    }

    void Update()
    {
        _transform.RotateAround(_rotationPoint, _rotationAxis, _rotationSpeed * Time.deltaTime);
    }
}
