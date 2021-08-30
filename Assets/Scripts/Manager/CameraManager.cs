using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public Camera gameplayCamera;
    public CinemachineVirtualCamera framingCamera;
    public CinemachineVirtualCamera threePersonCamera;
    
    private void OnEnable()
    {
        
    }

    public Camera GetGameplayCamera()
    {
        return gameplayCamera.GetComponent<Camera>();
    }

    public void LookAtPlayer(Transform playerTransform)
    {
        framingCamera.LookAt = playerTransform;
    }

    public void FollowPlayer(Transform playerTransform)
    {
        framingCamera.Follow = playerTransform;
    }

    
}
