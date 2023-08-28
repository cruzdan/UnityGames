using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransformOnXZAxis : MonoBehaviour
{
    [SerializeField] private Transform _transformToFollow;
    float _distanceZ;
    float _distanceX;
    Vector3 _auxiliarVector = new();
    private void Awake()
    {
        _transformToFollow = GameManager.Instance.GetPlayer().transform;
        enabled = false;
    }

    void Update()
    {
        CalculateNextPosition();
        SetNextPosition();
    }

    void CalculateNextPosition()
    {
        _auxiliarVector.z = _transformToFollow.position.z + _distanceZ;
        _auxiliarVector.x = _transformToFollow.position.x + _distanceX;
        _auxiliarVector.y = transform.position.y;
    }

    void SetNextPosition()
    {
        transform.position = _auxiliarVector;
    }

    public void RestartVars()
    {
        ResetDistances();
    }

    void ResetDistances()
    {
        _auxiliarVector = transform.position;
        _distanceZ = _auxiliarVector.z - _transformToFollow.position.z;
        _distanceX = _auxiliarVector.x - _transformToFollow.position.x;
    }
}
