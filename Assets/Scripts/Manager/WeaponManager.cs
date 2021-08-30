
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public GameObject[] _weaponePrefab;

    private List<int> _weaponHash = new List<int>();

    private void OnEnable()
    {
        foreach (var weapon in _weaponePrefab)
        {
            _weaponHash.Add(weapon.name.GetHashCode());
        }
    }
}
