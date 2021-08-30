using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class GlobalManager : Singleton<GlobalManager>
    {
        public GameObject _player;
        public PlayerController _pc;

        public void SetPlayerPawn(GameObject player)
        {
            _player = player;
        }
    }
}