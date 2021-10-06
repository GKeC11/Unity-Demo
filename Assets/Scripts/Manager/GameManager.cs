using System;
using UI.MainMenu;
using UnityEngine;
using Utils;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public NetworkManager _networkManager;

        public GameObject _mainMenuPrefab;

        public MessageManager _messageManager;

        public bool isGameManagerInitializeFinish;

        public GameObject _mainMenu;

        private void Start()
        {
            InitializeGame();
            InitializeManager();

            isGameManagerInitializeFinish = true;
        }

        void InitializeGame()
        {
            Instantiate(_networkManager);
            _mainMenu = Instantiate(_mainMenuPrefab);
            Instantiate(_messageManager);
        }

        public void ReInitializeMainMenu()
        {
            if (_mainMenu == null || !_mainMenu.GetComponent<MainMenuController>())
            {
                Instantiate(_mainMenuPrefab);
            }
        }

        void InitializeManager()
        {
            NetworkManager._instance.InitializeNetworkManager();
        }
    }
}
