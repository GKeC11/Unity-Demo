using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviourPunCallbacks
{

    public GameObject _itemToSpawn;

    public GameObject _refGameObject;

    private bool isSpawning;
    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return; 
        
        if (_refGameObject == null && !isSpawning)
        {
            StartCoroutine(ItemSpawnCoroutine());
        }
    }

    IEnumerator ItemSpawnCoroutine()
    {
        isSpawning = true;
        
        yield return new WaitForSeconds(5.0f);

        _refGameObject = PhotonNetwork.Instantiate("Item/" + _itemToSpawn.name, gameObject.transform.position, gameObject.transform.rotation);

        isSpawning = false;
    }
}
