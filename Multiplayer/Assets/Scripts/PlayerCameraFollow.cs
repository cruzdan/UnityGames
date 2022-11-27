using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraFollow : Singleton<PlayerCameraFollow>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void FollowPlayer(Transform followTransform)
    {
        cinemachineVirtualCamera.Follow = followTransform;
    }
}
