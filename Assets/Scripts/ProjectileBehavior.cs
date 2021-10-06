using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviourPunCallbacks
{
    public Rigidbody _rigidbody;
    public float bulletForce;
    public GunBehavior _spawner;
    public float _lifeTime;

    private void Start()
    {
        _rigidbody.AddForce(transform.forward * bulletForce);

        StartCoroutine(LifeTimeCoroutine());
    }
    
    IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
 
    }
    
    [PunRPC]
    public void PUNDestroy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
