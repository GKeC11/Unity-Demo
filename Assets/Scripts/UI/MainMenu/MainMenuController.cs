using System;
using System.Collections.Generic;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        public GameObject _loadingPanel;
        public GameObject _menuButtons;
        public TMP_Text _loadingText;
        public InputField _roomNameInputField;
        public GameObject _createRoomPanel;
        
        public GameObject _roomBrowserPanel;
        public GameObject _roomBrowserContent;
        public RoomCardController _roomCardPrefab;

        [Header("Room")]
        public GameObject _roomPanel;
        public Text _roomNameText;
        public Button _startGameButton;
        
        [Header("RoomPlayerList")] 
        public GameObject _roomPlayerContent;

        public PlayerCardController _playerCardPrefab;

        private List<PlayerCardController> _playerCardList;
        private List<RoomCardController> _roomCardList;

        [Header("Scene")] 
        public string _scene1;

        public InputField _nickNameInputField;

        private void Awake()
        {
            HideMenu();
            _loadingPanel.SetActive(true);

            NetworkManager._onConnectServer += PUNOnConnectSever;
            NetworkManager._onJoinLobby += PUNOnJoinLobby;
            NetworkManager._afterJoinLobby += PUNOnFinishInitialize;
            NetworkManager._onJoinRoom += PUNOnJoinRoom;
            NetworkManager._onCreateRoomFailed += PUNOnCreateRoomFailed;
            NetworkManager._onLeaveRoom += PUNOnLeaveRoom;
            NetworkManager._afterLeaveRoom += PUNAfterLeaveRoom;
            NetworkManager._onRoomListUpdate += PUNUpdateRoomList;
            NetworkManager._onCreateRoom += PUNOnCreateRoom;
            NetworkManager._onPlayerEnterRoom += PUNOnRoomPlayerEnterOrLeave;
            NetworkManager._onPlayerLeftRoom += PUNOnRoomPlayerEnterOrLeave;
            NetworkManager._onMasterClientSwitch += PUNOnUpdateRoom;

            _roomCardList = new List<RoomCardController>();
            _playerCardList = new List<PlayerCardController>();
            
            _nickNameInputField.onValueChanged.AddListener(UpdateNickName); 
        }

        #region ClickEvent
        // 点击事件

        public void ClickEventQuit()
        {
            Application.Quit();
        }

        public void ClickEventFindRoom()
        {
            HideMenu();
            
            _roomBrowserPanel.SetActive(true);
        }

        public void ClickEventShowCreateRoomPanel()
        {
            _createRoomPanel.SetActive(true);
        }

        public void ClickEventHideCreateRoomPanel()
        {
            _createRoomPanel.SetActive(false);
        }

        public void ClickEventCreateRoom()
        {
            var str = _roomNameInputField.text;
            NetworkManager._instance.CreateRoom(str);
        }

        public void ClickEventLeaveRoom()
        {
            NetworkManager._instance.LeaveRoom();
        }
        
        public void ClickEventCloseRoomBrowserPanel()
        {
            HideMenu();
            
            _menuButtons.SetActive(true);
        }

        public void ClickEventStartGame()
        {
            NetworkManager._instance.StartGame(_scene1);
        }

        public void UpdateNickName(string nickName)
        {
            PhotonNetwork.NickName = nickName;
        }
        
        #endregion
        
        
        #region PUNEvent
        // 处理 PUN 相关的事件

        void PUNOnCreateRoom()
        {
            HideMenu();
            
            _loadingPanel.SetActive(true);
            _loadingText.text = "创建房间中 ……";
        }
        
        void PUNOnConnectSever()
        {
            _loadingText.SetText("正在连接服务器 ……");
        }

        void PUNOnJoinLobby()
        {
            _loadingText.SetText("正在连接大厅");
        }

        void PUNOnFinishInitialize()
        {
            HideMenu();
            
            _menuButtons.SetActive(true);
        }

        void PUNOnJoinRoom(string roomName)
        {
            HideMenu();
            
            _roomPanel.SetActive(true);
            _roomNameText.text = roomName;
            
            UpdateRoomPlayerList();
            
            _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        void PUNOnUpdateRoom()
        {
            _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
        }

        void PUNOnRoomPlayerEnterOrLeave()
        {
            UpdateRoomPlayerList();
        }

        void PUNOnCreateRoomFailed(string message, int returnCode)
        {
            var uiPrefab = MessageManager._instance._uiErrorMessage;
            var uiMessage = Instantiate(uiPrefab, transform);
            uiMessage._messageText.text = $"{message}{returnCode}";
            uiMessage._buttonAction += () =>
            {
                HideMenu();
                
                _menuButtons.SetActive(true);
                Destroy(uiMessage.gameObject);
            };
        }

        void PUNOnLeaveRoom()
        {
            HideMenu();
            
            _loadingPanel.SetActive(true);
            _loadingText.text = "正在离开房间 ……";
        }

        void PUNAfterLeaveRoom()
        {
            // HideMenu();
            //
            // _menuButtons.SetActive(true);
        }

        void PUNUpdateRoomList(List<RoomInfo> roomInfos)
        {
            foreach (var roomCard in _roomCardList)
            {
                Destroy(roomCard.gameObject);
            }
            _roomCardList.Clear();

            foreach (var roomInfo in roomInfos)
            {
                if (roomInfo.MaxPlayers < 1) continue;
                
                var roomCard = Instantiate(_roomCardPrefab, _roomBrowserContent.transform);
                roomCard._roomNameText.text = roomInfo.Name;
                _roomCardList.Add(roomCard);
            }
        }

        #endregion

        void UpdateRoomPlayerList()
        {
            foreach (var playerCard in _playerCardList)
            {
                Destroy(playerCard.gameObject);
            }
            _playerCardList.Clear();

            var playerInfos = PhotonNetwork.CurrentRoom.Players;
            foreach (var playerInfo in playerInfos)
            {
                var playerCard = Instantiate(_playerCardPrefab, _roomPlayerContent.transform);
                playerCard._playerNameText.text = playerInfo.Value.NickName;
                _playerCardList.Add(playerCard);
            }
        }
        
        void HideMenu()
        {
            _loadingPanel.SetActive(false);
            _menuButtons.SetActive(false);
            _createRoomPanel.SetActive(false);
            _roomPanel.SetActive(false);
            _roomBrowserPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            NetworkManager._onConnectServer -= PUNOnConnectSever;
            NetworkManager._onJoinLobby -= PUNOnJoinLobby;
            NetworkManager._afterJoinLobby -= PUNOnFinishInitialize;
            NetworkManager._onJoinRoom -= PUNOnJoinRoom;
            NetworkManager._onCreateRoomFailed -= PUNOnCreateRoomFailed;
            NetworkManager._onLeaveRoom -= PUNOnLeaveRoom;
            NetworkManager._afterLeaveRoom -= PUNAfterLeaveRoom;
            NetworkManager._onRoomListUpdate -= PUNUpdateRoomList;
            NetworkManager._onCreateRoom -= PUNOnCreateRoom;
            NetworkManager._onPlayerEnterRoom -= PUNOnRoomPlayerEnterOrLeave;
            NetworkManager._onPlayerLeftRoom -= PUNOnRoomPlayerEnterOrLeave;
            NetworkManager._onMasterClientSwitch -= PUNOnUpdateRoom;
        }
    }
}
