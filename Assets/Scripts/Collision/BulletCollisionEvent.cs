using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletCollisionEvent : BaseColliderEvent
{
    public GameObject _impactEffect;
    public GameObject _root;

    private void OnCollisionEnter(Collision other)
    {
        // Instantiate(_impactEffect, other.contacts[0].point, Quaternion.LookRotation(other.impulse, Vector3.up));
        // Destroy(_root);
        var damager = _root.GetComponent<PhotonView>().Owner;
        var spawner = _root.GetComponent<ProjectileBehavior>()._spawner;
        other.collider.gameObject.GetComponent<CharacterCollisionEvent>()?.OnBulletHit(other, damager, spawner, _root);
        // Destroy(_root);
        // _root.GetComponent<ProjectileBehavior>().PUNDestroy();
    }
}
