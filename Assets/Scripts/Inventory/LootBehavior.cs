using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LootBehavior : MonoBehaviourPunCallbacks
{
    public int _itemID;
    public int _ammout = 1;
    
    private void OnTriggerEnter(Collider other)
    {

        var inventory = other.gameObject.GetComponent<Inventory>();
        if (inventory != null)
        {
            Debug.Log("获得背包");
            inventory.AddItemToSlot(_itemID, _ammout);
            Pickup();
        }

    }

    [PunRPC]
    void Pickup()
    {
        Destroy(gameObject);
    }
}
