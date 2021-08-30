using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponBehavior : MonoBehaviour
{
    public GameObject weaponSlot;
    public List<GameObject> weaponList;

    public GameObject currentWeapon;
    private GunBehavior gunBehavior;
    
    private void OnEnable()
    {
        //Test
        var prefab = WeaponManager.Instance._weaponePrefab[0];
        var weapon = Instantiate(prefab, weaponSlot.transform);
        
        weaponList.Add(weapon);
        currentWeapon = weapon;
    }

    public void Attack(InputActionPhase actionPhase)
    {
        // gunBehavior.Fire(actionPhase);
    }

    public void ToggleWeaponSlotVisible()
    {
        weaponSlot.SetActive(!weaponSlot.activeSelf);
    }

    public void ToggleAllWeaponVisible()
    {
        
    }

}
