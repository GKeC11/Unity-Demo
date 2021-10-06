using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Manager
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager _instance;

        public bool _isDontDestroyOnLoad;

        public static Action _onConnectServer;
        public static Action _onJoinLobby;
        public static Action _afterJoinLobby;
        public static Action<string> _onJoinRoom;
        public static Action<string, int> _onCreateRoomFailed;
        public static Action _onLeaveRoom;
        public static Action _afterLeaveRoom;
        public static Action<List<RoomInfo>> _onRoomListUpdate;
        public static Action _onCreateRoom;
        public static Action _onPlayerEnterRoom;
        public static Action _onPlayerLeftRoom;
        public static Action _onMasterClientSwitch;
        

        private void Awake()
        {
            if (!InitializeSingleton()) return;
            Debug.Log("NetworkManager Initialize Success");
        }

        public void InitializeNetworkManager()
        {
            _onConnectServer?.Invoke();
            
            PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cn";
            PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
            PhotonNetwork.PhotonServerSettings.AppSettings.Server = "ns.photonengine.cn";
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom(string roomName)
        {
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.Log(PhotonNetwork.NetworkingClient.State);
                
                return;
            }

            var roomOptions = new RoomOptions {MaxPlayers = 8};
            PhotonNetwork.CreateRoom(roomName, roomOptions);

            _onCreateRoom?.Invoke();
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            _onLeaveRoom?.Invoke();
        }

        public void JoinRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void StartGame(string sceneName)
        {
            PhotonNetwork.LoadLevel(sceneName);
        }

        #region PUN Callback

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            _onPlayerEnterRoom?.Invoke();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            _onPlayerLeftRoom?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"无法连接到服务器： {cause}");
            Debug.Log("正在尝试重连……");
            
            PhotonNetwork.Reconnect();
        }

        public override void OnLeftRoom()
        {
            _afterLeaveRoom?.Invoke();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("房间列表更新");
            _onRoomListUpdate?.Invoke(roomList);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _onCreateRoomFailed?.Invoke(message, returnCode);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"{message}{returnCode}");
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("进入房间");
            var str = PhotonNetwork.CurrentRoom.Name;

            _onJoinRoom?.Invoke(str);
        }

        public override void OnConnectedToMaster()
        {
            _onJoinLobby?.Invoke();
            PhotonNetwork.JoinLobby();

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("进入大厅");
            _afterJoinLobby?.Invoke();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            _onMasterClientSwitch?.Invoke();
        }

        #endregion

        bool InitializeSingleton()
        {
            if (_instance != null && _instance == this) return true;

            if (_isDontDestroyOnLoad)
            {
                if (_instance != null)
                {
                    Destroy(gameObject);
                    return false;
                }

                _instance = this;
                if (Application.isPlaying) DontDestroyOnLoad(gameObject);
            }
            else
            {
                _instance = this;
            }

            return true;
        }
    }
}