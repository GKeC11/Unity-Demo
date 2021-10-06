
using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Action _addItemAction;
    public int _curSelect;
    
    public class InventorySlot
    {
        public InventoryItem item;
        public int amount;
        public GameObject objectReference;
    }

    public int _inventorySize;
    public List<InventorySlot> _inventorySlots;

    private UIController _uiController;
    
    private void Awake()
    {
        
        _inventorySlots = new List<InventorySlot>(_inventorySize);
        for (int i = 0; i < _inventorySize; i++)
        {
            _inventorySlots.Add(new InventorySlot {item = null, amount = 0});
        }
    }

    public void AddItemToSlot(int itemID, int amount = 1)
    {
        var index = FindEmptySlot();
        if (index >= 0)
        {
            _inventorySlots[index].item = InventoryItem.GetItemFormID(itemID);
            _inventorySlots[index].amount = amount;

            // _uiController = GlobalManager._instance._gameplayCanvas.GetComponent<UIController>();
            // _uiController._inventoryUI.UpdateInventoryUI();
            _addItemAction?.Invoke();
        }
        
    }

    private int FindEmptySlot()
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_inventorySlots[i].item == null)
            {
                return i;
            }
        }

        return -1;
    }

    public InventorySlot FindItem(InventoryItem item)
    {
        foreach (var slot in _inventorySlots)
        {
            if (slot.item == item)
            {
                return slot;
            }
        }

        return null;
    }

    public int FindItemIndex(InventoryItem item)
    {
        var index = -1;
        foreach (var slot in _inventorySlots)
        {
            index++;
            if (slot.item == item)
            {
                return index;
            }
        }

        return -1;
    }

    public void DropItem()
    {
        if (_uiController == null)
        {
            _uiController = GameplayManager._instance._gameplayUI.GetComponent<UIController>();
        }
        
        if (_inventorySlots[_curSelect] != null)
        {
            _inventorySlots.RemoveAt(_curSelect);
        }
        
        _uiController._inventoryUI.UpdateInventoryUI();
    }

    public int RemoveItem(InventoryItem item, int amount = 1)
    {
        if (_uiController == null)
        {
            _uiController = GameplayManager._instance._gameplayUI.GetComponent<UIController>();
        }
        
        var index = FindItemIndex(item);
        InventorySlot slot = null;
        
        if (index >= 0)
        {
            slot = _inventorySlots[index];
        }
        if (slot != null && slot.item != null)
        {
            if (slot.amount > amount)
            {
                slot.amount -= amount;
                _uiController._inventoryUI.UpdateInventoryUI();
                return amount;
            }

            // 物品数量少于所需数量
            var left = slot.amount;
            slot.item = null;
            slot.amount = 0;
            slot.objectReference = null;
            _uiController._inventoryUI.UpdateInventoryUI();
            return left;
        }
        
        return -1;
    }
}
