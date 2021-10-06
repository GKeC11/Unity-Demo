using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class CharacterCollisionEvent : MonoBehaviourPunCallbacks
{
    public GameObject _bulletHitPrefab;

    private PlayerAttributeSystem _playerAttributeSystem;

    public AudioClip _damageSFX;

    private AudioSource _audioSource;
    private void Start()
    {
        _playerAttributeSystem = gameObject.GetComponent<PlayerAttributeSystem>();
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnBulletHit(Collision collision, Player damager, GunBehavior spawner, GameObject other)
    {
        Instantiate(_bulletHitPrefab, collision.contacts[0].point, Quaternion.identity);

        _audioSource.clip = _damageSFX;
        _audioSource.Play();
        
        if (!photonView.IsMine) return;

        // _playerAttributeSystem.photonView.RPC("ChangeCurrentHealth", RpcTarget.All, -spawner._damage);
        _playerAttributeSystem.TakeHit(damager, spawner);
        
        other.GetComponent<ProjectileBehavior>().PUNDestroy();
        // other.GetComponent<ProjectileBehavior>().PUNDestroy();
        // _playerAttributeSystem.ChangeCurrentHealth(-spawner._damage);
    }
}
