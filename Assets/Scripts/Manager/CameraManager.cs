using System;
using Cinemachine;
using Manager.Utils;
using UnityEngine;
using Utils;

namespace Manager
{
    public class CameraManager : Singleton<CameraManager>, IManager
    {
        public Camera gameplayCamera;
        public CinemachineStateDrivenCamera _stateDrivenCamera;

        public Camera GetGameplayCamera()
        {
            return gameplayCamera.GetComponent<Camera>();
        }

        public void Initialize()
        {
            
        }
    }
}
