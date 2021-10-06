using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Manager
{
    public class GlobalManager : Singleton<GlobalManager>
    {
        public GameObject _player;
        public PlayerController _pc;
        public Canvas _gameplayCanvas;

        public void SetPlayerPawn(GameObject player)
        {
            
            _player = player;
        }

        public Inventory GetInventory()
        {
            var inventory = _pc.GetComponent<Inventory>();
            return inventory;
        }

        public void Setup(GameObject player, Canvas canvas)
        {
            _player = player;
            _pc = player.GetComponent<PlayerController>();
            _gameplayCanvas = canvas;
        }
    }
}