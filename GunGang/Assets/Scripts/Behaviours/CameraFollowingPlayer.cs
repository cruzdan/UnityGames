using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    [SerializeField] private Transform _transformToFollow;
    [SerializeField] private Transform _camera;
    private Vector3 _position;

    private void Start()
    {
        _position = _transformToFollow.position;
        _camera.SetParent(_transformToFollow);
    }

    void Update()
    {
        _position.z = transform.position.z;
        _transformToFollow.position = _position;
    }
}
