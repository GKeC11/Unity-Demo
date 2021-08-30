
using Manager;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Combat,
}

public class PlayerBehavior : MonoBehaviour
{
    public PlayerState _playerState = PlayerState.Normal;
    
    public Animator _animator;

    [HideInInspector] 
    public int _targetState;

    [HideInInspector] 
    public int _layerIndex;
    
    [HideInInspector] 
    public int _stateIndex;
    
    [HideInInspector]
    public MySerializedDictionary<int, int> _stateParentLookup;

    private void OnEnable()
    {

        CameraManager.Instance.FollowPlayer(GlobalManager.Instance._pc.followTarget.transform);
    }

    private void Update()
    {
        var s = _animator.GetCurrentAnimatorStateInfo(_layerIndex);

        if (_targetState != s.fullPathHash && _stateParentLookup != null)
        {
            int hash = s.fullPathHash;
            while (hash != 0 && _stateParentLookup.ContainsKey(s.fullPathHash))
            {
                hash = _stateParentLookup.ContainsKey(hash) ? _stateParentLookup[hash] : 0;
                if (_targetState == hash)
                {
                    _playerState = PlayerState.Combat;
                    break;
                }

                _playerState = PlayerState.Normal;
            }
        }
    }
}