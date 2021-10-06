using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Item"))]
public class InventoryItem : ScriptableObject
{
    // Editable
    public int _itemID;

    public string _name;

    [TextArea] public string _description;

    public int _maxStack;

    public Sprite _itemIcon;

    public GameObject _prefab;


    static Dictionary<int, InventoryItem> _itemLookup;

    public static InventoryItem GetItemFormID(int itemID)
    {
        if (_itemLookup == null)
        {
            _itemLookup = new Dictionary<int, InventoryItem>();

            var list = Resources.LoadAll<InventoryItem>("ScriptableObjects/Inventory");
            foreach (var item in list)
            {
                if (_itemLookup.ContainsKey(item._itemID))
                {
                    Debug.Log($"存在相同 Guid 的物品 {_itemLookup[item._itemID]}, {item}");
                    continue;
                }

                _itemLookup[item._itemID] = item;
            }
        }

        if (itemID == 0 || !_itemLookup.ContainsKey(itemID)) return null;
        return _itemLookup[itemID];
    }
    
    
}