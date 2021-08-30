using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Rigidbody _rigidbody;
    public float bulletForce;

    private void OnEnable()
    {
        _rigidbody.AddForce(transform.forward * bulletForce);
    }

    
}
