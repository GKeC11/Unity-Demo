using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image _itemIcon;
    public Image _bg;
    public TMP_Text _amount;

    private int _index;
    private Inventory _inventory;
    private InventoryItem _item = null;

    private void OnEnable()
    {
    }

    public void InitInventorySlotUI(Inventory inventory, int index)
    {
        // _item = inventory._inventorySlots[index].item;
        Inventory.InventorySlot slot = null;
        if (inventory._inventorySlots.Count > index)
        {
            slot = inventory._inventorySlots[index];
            _item = slot.item;
        }

        if (_item == null)
        {
            _itemIcon.enabled = false;
        }
        else
        {
            _itemIcon.enabled = true;
            _itemIcon.sprite = _item._itemIcon;
        }
        
        // 数量
        if (slot?.amount > 1)
        {
            _amount.enabled = true;
            _amount.SetText(slot.amount.ToString());
        }
        else
        {
            _amount.enabled = false;
        }

        _inventory = inventory;
        _index = index;
    }

    public void CheckSelected(int select)
    {
        if (_index == select && _item != null)
        {
            if (_item._prefab != null)
            {
                var gunBehavior = _item._prefab.GetComponent<GunBehavior>();
                var playerWeaponBehavior = GameplayManager._instance._player.GetComponent<PlayerWeaponBehavior>();
                if (gunBehavior != null)
                {
                    Debug.Log("武器！");
                    
                    var inventoryInventorySlot = _inventory._inventorySlots[_index];
                    if (inventoryInventorySlot.objectReference == null)
                    {
                        // playerWeaponBehavior.photonView.RPC("EquippingWeapon", RpcTarget.All, _item._itemID, _index);
                        playerWeaponBehavior.EquippingWeapon(_item._itemID, _index);
                    }
                    else
                    {
                        playerWeaponBehavior.photonView.RPC("EquippingWeapon", RpcTarget.All, _index);
                        // playerWeaponBehavior.EquippingWeapon(_index);
                    }
                }
            }

            _bg.color = Color.cyan;
        }
        else
        {
            _bg.color = Color.white;
        }
    }
}