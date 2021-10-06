using System;
using System.Collections.Generic;
using Manager.Utils;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Manager
{
    public class LoadManager : Singleton<LoadManager>
    {
        
        public List<GameObject> Managers;

        protected override void Awake()
        {
            base.Awake();

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("NetworkTest");
            }
        }

        private void Start()
        {
            foreach (var manager in Managers)
            {
                var managerComponent = manager.GetComponent<IManager>();
                managerComponent.Initialize();
            }
        }
    }
}
