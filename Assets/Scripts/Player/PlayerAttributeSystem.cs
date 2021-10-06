using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using Math = UnityEngine.ProBuilder.Math;

public class PlayerAttributeSystem : MonoBehaviourPunCallbacks, IPlayerSystem, IOnEventCallback
{

    public int _maxHealth = 100;
    public int _curHealth;

    private PlayerBehavior _playerBehavior;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    
    private void Awake()
    {
        
        
        
    }

    public void TakeHit(Player damager, GunBehavior spawner)
    {
        if (!photonView.IsMine) return;

        _curHealth = Math.Clamp(_curHealth - spawner._damage, 0, _maxHealth);

        object[] data = { _curHealth, photonView.ViewID, damager.NickName, PhotonNetwork.LocalPlayer.NickName };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(EventCodeManager.TakeHitCode, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public void Initialize()
    {
        _curHealth = _maxHealth;
        _playerBehavior = GetComponent<PlayerBehavior>();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (EventCodeManager.TakeHitCode == photonEvent.Code)
        {
            object[] data = (object[])photonEvent.CustomData;

            var viewID = (int) data[1];
            
            if(viewID == photonView.ViewID)
            { 
                _curHealth = (int) data[0];

                if (_curHealth <= 0)
                {
                    _playerBehavior.Die((string) data[2], (string) data[3]);
                }
            }
        }
    }
    
}
