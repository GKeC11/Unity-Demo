using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public InventorySlotUI _inventorySlotPrefab;

    private Inventory _inventory;

    private void Awake()
    {
        Inventory._addItemAction += UpdateInventoryUI;
    }

    private void Start()
    {
        InitInventoryUI();
    }

    public void InitInventoryUI()
    {
        
        _inventory = GameplayManager._instance._player.GetComponent<Inventory>();

        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _inventory._inventorySize; i++)
        {
            var inventorySlotUI = Instantiate(_inventorySlotPrefab, transform);
            inventorySlotUI.InitInventorySlotUI(_inventory, i);
        }
    }

    public void SelectSlot(int index)
    {

        foreach (Transform child in transform)
        {
            var slotUI = child.GetComponent<InventorySlotUI>();
            slotUI.CheckSelected(index - 1);
            _inventory._curSelect = index - 1;
        }
    }

    public void ClearSelect()
    {
        foreach (Transform child in transform)
        {
            var slotUI = child.GetComponent<InventorySlotUI>();
            slotUI.CheckSelected(-1);
        }
    }
}
