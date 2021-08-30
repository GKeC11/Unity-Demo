using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseColliderEvent : MonoBehaviour
{
    public MyColliderBehavior _colliderBehavior;

    public Dictionary<string, bool> _ignoreDictionary;

    public Dictionary<string, bool> GetIgnoreDictionary()
    {
        if (_ignoreDictionary == null)
        {
            _ignoreDictionary = new Dictionary<string, bool>();
            foreach (var pair in _colliderBehavior._ignoreDictionary)
            {
                _ignoreDictionary.Add(pair.Key, pair.Value);
            }
        }

        return _ignoreDictionary;
    }
}