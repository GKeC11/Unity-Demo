using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSV2ScriptObject : MonoBehaviour
{
    private static string _itemCSVPath = "/Files/CSV/Items.csv";
    private static string _itemScriptObjectPath = "Assets/Resources/ScriptableObjects/Inventory/";
    
    [MenuItem("Utilities/CreateItemObject")]
    public static void CreateItemObject()
    {
        string[] lines = File.ReadAllLines(Application.dataPath + _itemCSVPath);

        foreach (var line in lines)
        {
            string[] datas = line.Split(',');

            if (datas.Length != 5)
            {
                Debug.Log($"数据错误 {line}");
                return;
            }

            // 例如 "Revolver.asset";
            var path = $"{_itemScriptObjectPath}{datas[4]}.asset";
            var oldItem = AssetDatabase.LoadAssetAtPath<InventoryItem>(path);
            var item = ScriptableObject.CreateInstance<InventoryItem>();
            item._itemID = int.Parse(datas[0]);
            item._name = datas[1];
            item._description = datas[2];
            item._maxStack = int.Parse(datas[3]);
            if (oldItem != null)
            {
                item._itemIcon = oldItem._itemIcon;
                item._prefab = oldItem._prefab;
            }
            
            AssetDatabase.CreateAsset(item, path);
        }
        
        AssetDatabase.SaveAssets();
    }
    
}
