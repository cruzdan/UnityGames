using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformOnZAxis : MonoBehaviour
{
    [SerializeField] private Transform _followTransform;
    float _distanceZ;
    Vector3 _auxiliarPosition = new();
    private Transform _transform;

    void Start()
    {
        _transform = transform;
        _auxiliarPosition = _transform.position;
        _distanceZ = _auxiliarPosition.z - _followTransform.position.z;
    }

    void Update()
    {
        _auxiliarPosition.z = _followTransform.position.z + _distanceZ;
        _transform.position = _auxiliarPosition;
    }
}
