using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTransformOnAxis : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _movementAxis;
    Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.position += Time.deltaTime * _movementAxis;
    }
}
