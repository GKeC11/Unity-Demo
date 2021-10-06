
using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Photon.Pun;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Combat,
    Dead,
}

public class PlayerBehavior : MonoBehaviourPunCallbacks, IPlayerBehavior
{
    public PlayerState _playerState = PlayerState.Normal;

    public Animator _animator;

    public GameObject _followTarget;

    [HideInInspector] public int _layerIndex;

    [HideInInspector] public MySerializedDictionary<int, int> _stateParentLookup;

    [HideInInspector]
    public List<AnimatorStateWithPlayerStatePair> _targetStates;
    
    [Serializable]
    public struct AnimatorStateWithPlayerStatePair
    {
        public int _targetState;
        public int _playerState;
    }

    private PlayerAnimatorBehavior _playerAnimatorBehavior;

    private void Start()
    {
        
    }

    private bool isChange = false;
    private void Update()
    {
        isChange = false;
        
        foreach (var targetState in _targetStates)
        {
            CheckState(targetState._targetState, targetState._playerState);
            if(isChange) break;
        }
    }

    private void CheckState(int state, int playerStateIndex)
    {
        var s = _animator.GetCurrentAnimatorStateInfo(_layerIndex);

        if (state != s.fullPathHash && _stateParentLookup != null)
        {
            int hash = s.fullPathHash;
            while (hash != 0 && _stateParentLookup.ContainsKey(s.fullPathHash))
            {
                hash = _stateParentLookup.ContainsKey(hash) ? _stateParentLookup[hash] : 0;
                if (state == hash)
                {
                    _playerState = (PlayerState) playerStateIndex;
                    isChange = true;
                    break;
                }

                _playerState = PlayerState.Normal;
            }
        }

        if (state == s.fullPathHash)
        {
            _playerState = (PlayerState) playerStateIndex;
            isChange = true;
        }
    }

    [PunRPC]
    public void DealHit()
    {
        
    }

    public void Die(string killer, string killed)
    {
        if (_playerState == PlayerState.Dead) return;

        _playerState = PlayerState.Dead;

        _playerAnimatorBehavior.Die();
        
        GameplayManager._instance._gameplayUI.GetComponent<UIController>().UpdateKillInfo(killer, killed);

        if (!photonView.IsMine) return;
        
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(2.0f);

        PhotonNetwork.Destroy(this.gameObject);
        
        GameplayManager._instance.Respawn();
    }

    public void Initialize()
    {
        _playerAnimatorBehavior = GetComponent<PlayerAnimatorBehavior>();
    }
}