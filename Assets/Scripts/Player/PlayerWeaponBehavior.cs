using System.Collections.Generic;
using ExitGames.Client.Photon;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponBehavior : MonoBehaviourPunCallbacks, IPlayerBehavior, IOnEventCallback
{
    public GameObject weaponSlot;
    public GameObject currentWeapon;

    private List<GameObject> _weaponCache;
    private GunBehavior _gunBehavior;
    private PlayerAnimatorBehavior _animatorBehavior;
    private UIController _uiController;

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
    }

    public void Attack(InputActionPhase actionPhase)
    {
        if (_gunBehavior != null)
        {
            _gunBehavior.Fire(actionPhase);
        }
    }

    private void ToggleWeaponSlotVisible()
    {
        weaponSlot.SetActive(!weaponSlot.activeSelf);
    }

    private void MakeAllWeaponInvisible()
    {
        foreach (var weapon in _weaponCache)
        {
            weapon.SetActive(false);
        }
    }

    public void EquippingWeapon(int itemID, int slotIndex)
    {
        if (_uiController == null)
        {
            _uiController = GameplayManager._instance._gameplayUI.GetComponent<UIController>();
        }

        MakeAllWeaponInvisible();

        var item = InventoryItem.GetItemFormID(itemID);
        var slot = GetComponent<Inventory>()._inventorySlots[slotIndex];

        var weapon = Instantiate(item._prefab, weaponSlot.transform);
        PhotonNetwork.AllocateViewID(weapon.GetComponent<PhotonView>());
        PhotonNetwork.RaiseEvent(EventCodeManager.InstantiateWeaponCode,
            new object[] {weapon.GetComponent<PhotonView>().ViewID, itemID, slotIndex, photonView.ViewID},
            new RaiseEventOptions {Receivers = ReceiverGroup.Others}, SendOptions.SendReliable);

        weapon.GetComponent<GunBehavior>()._owner = this.gameObject;
        slot.objectReference = weapon;

        currentWeapon = weapon;

        _gunBehavior = weapon.GetComponent<GunBehavior>();
        weapon.SetActive(true);

        if (photonView.IsMine)
        {
            _uiController.UpdateWeaponData(currentWeapon);
        }

        _animatorBehavior.UpdateAnimatorState(GunType2AnimatorStateType(_gunBehavior._type));

        if (!_weaponCache.Contains(weapon))
        {
            _weaponCache.Add(weapon);
        }
    }

    [PunRPC]
    public void EquippingWeapon(int slotIndex)
    {
        if (_uiController == null)
        {
            _uiController = GameplayManager._instance._gameplayUI.GetComponent<UIController>();
        }

        MakeAllWeaponInvisible();

        var weapon = GetComponent<Inventory>()._inventorySlots[slotIndex].objectReference;
        currentWeapon = weapon;

        _gunBehavior = weapon.GetComponent<GunBehavior>();
        weapon.SetActive(true);

        if (photonView.IsMine)
        {
            _uiController.UpdateWeaponData(currentWeapon);
        }

        _animatorBehavior.UpdateAnimatorState(GunType2AnimatorStateType(_gunBehavior._type));

        if (!_weaponCache.Contains(weapon))
        {
            _weaponCache.Add(weapon);
        }
    }

    [PunRPC]
    public void DisArm()
    {
        if (_uiController == null)
        {
            _uiController = GameplayManager._instance._gameplayUI.GetComponent<UIController>();
        }

        MakeAllWeaponInvisible();
        currentWeapon = null;
        _gunBehavior = null;

        _uiController.ClearSelect();
        _animatorBehavior.UpdateAnimatorState(PlayerAnimatorBehavior.AnimatorState.Normal);
    }

    private PlayerAnimatorBehavior.AnimatorState GunType2AnimatorStateType(GunBehavior.GunType type)
    {
        switch (type)
        {
            case GunBehavior.GunType.Pistol:
                return PlayerAnimatorBehavior.AnimatorState.Pistol;
            case GunBehavior.GunType.Rifle:
                return PlayerAnimatorBehavior.AnimatorState.Rifle;
        }

        return PlayerAnimatorBehavior.AnimatorState.Normal;
    }

    [PunRPC]
    public void Reload()
    {
        if (_gunBehavior != null)
        {
            _gunBehavior.Reload();
        }
    }

    public void Initialize()
    {
        _weaponCache = new List<GameObject>();
        _animatorBehavior = gameObject.GetComponent<PlayerAnimatorBehavior>();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeManager.InstantiateWeaponCode)
        {
            var data = (object[]) photonEvent.CustomData;
            if (photonView.ViewID != (int) data[3]) return;
            
            var item = InventoryItem.GetItemFormID((int) data[1]);
            var slot = GetComponent<Inventory>()._inventorySlots[(int) data[2]];
            var weapon = Instantiate(item._prefab, weaponSlot.transform);
            weapon.GetComponent<PhotonView>().ViewID = (int) data[0];

            weapon.GetComponent<GunBehavior>()._owner = this.gameObject;
            slot.objectReference = weapon;

            currentWeapon = weapon;

            _gunBehavior = weapon.GetComponent<GunBehavior>();
            weapon.SetActive(true);

            _animatorBehavior.UpdateAnimatorState(GunType2AnimatorStateType(_gunBehavior._type));

            if (!_weaponCache.Contains(weapon))
            {
                _weaponCache.Add(weapon);
            }
        }
    }
}