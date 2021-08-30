using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MySerializedDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
{
    [SerializeField] List<K> _keys = new List<K>();

    [SerializeField] List<V> _values = new List<V>();


    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in this)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < _keys.Count; i++)
        {
            if (!ContainsKey(_keys[i]))
            {
                Add(_keys[i], _values[i]);
            }
            else
            {
                break;
            }
        }

        _keys.Clear();
        _values.Clear();
    }
}