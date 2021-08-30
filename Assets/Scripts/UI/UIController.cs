
using System;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMP_Text _ammoText;
    public GameObject _player;
    public Image _crossHair;

    private PlayerWeaponBehavior _playerWeaponBehavior;
    private PlayerBehavior _playerBehavior;

    private void OnEnable()
    {
        InitController();
    }
    
    private GunBehavior _gunBehavior;

    private void InitController()
    {
        if (_player == null)
        {
            _player = GlobalManager.Instance._player;
        }
        
        _playerWeaponBehavior = _player.GetComponent<PlayerWeaponBehavior>();
        _playerBehavior = _player.GetComponent<PlayerBehavior>();
        if (_playerWeaponBehavior.currentWeapon)
        {
            _gunBehavior = _playerWeaponBehavior.currentWeapon.GetComponent<GunBehavior>();
        }
    }

    private void ToggleCrossHairVisible()
    {

        switch (_playerBehavior._playerState)
        {
            case PlayerState.Normal:
                _crossHair.enabled = false;
                break;
            case PlayerState.Combat:
                _crossHair.enabled = true;
                break;
            default:
                _crossHair.enabled = false;
                break;
        }


    }


    private void Update()
    {
        ToggleCrossHairVisible();

        if (_gunBehavior != null)
        {
            var ammoStr = $"{_gunBehavior.currentAmmo} / {_gunBehavior.totalAmmo}";
            _ammoText.SetText(ammoStr);
        }
        
    }
}
