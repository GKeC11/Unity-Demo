using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionEvent : BaseColliderEvent
{
    public GameObject _impactEffect;
    public GameObject _root;

    private void OnCollisionEnter(Collision other)
    {
        Instantiate(_impactEffect, other.contacts[0].point, Quaternion.LookRotation(other.impulse, Vector3.up));
        Destroy(_root);
    }
}
