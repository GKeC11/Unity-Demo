using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Manager.Utils;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils;
using Random = UnityEngine.Random;

namespace Manager
{
    public class GameplayManager : Singleton<GameplayManager>, IManager
    {
        public List<GameObject> _playerPrefabs;

        public List<GameObject> _spawnPoints;

        public GameObject _player;

        public GameObject _gameplayUI;

        public bool isPause;

        private int _playerNumber;

        // Start is called before the first frame update
        void SetupGameplay()
        {
            _playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

            var random = Random.Range(0, 5);
            var point = _spawnPoints[random];
            
            _player = PhotonNetwork.Instantiate("Player/" + _playerPrefabs[_playerNumber % 3].name, point.transform.position,
                point.transform.rotation);

            _player.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        public void Initialize()
        {
            SetupGameplay();

            Cursor.lockState = CursorLockMode.Locked;
        }

        public void Respawn()
        {

            StartCoroutine(RespawnCoroutine());
        }

        IEnumerator RespawnCoroutine()
        {
            while (CameraManager._instance._stateDrivenCamera != null)
            {
                yield return new WaitForSeconds(1.0f);
            }
            
            var point = _spawnPoints[0];
            
            _player = PhotonNetwork.Instantiate("Player/" + _playerPrefabs[_playerNumber % 3].name, point.transform.position,
                point.transform.rotation);
                
            _gameplayUI.GetComponent<UIController>()._player = null;
            _gameplayUI.GetComponent<UIController>().InitController();
            _gameplayUI.GetComponent<UIController>()._inventoryUI.InitInventoryUI();
            
        }
        

        #region GameControl

        public void PauseMenu(InputAction.CallbackContext ctx)
        {
            PauseMenuAction();
        }

        public void PauseMenuAction()
        {
            if (!isPause)
            {
                Cursor.lockState = CursorLockMode.None;
                _gameplayUI.GetComponent<UIController>()._pauseMenu.SetActive(true);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                _gameplayUI.GetComponent<UIController>()._pauseMenu.SetActive(false);
            }

            isPause = !isPause;
        }

        #endregion
    }
}