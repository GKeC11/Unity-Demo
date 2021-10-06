
using System;
using Manager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TMP_Text _ammoText;
    public GameObject _player;
    public Image _crossHair;
    public InventoryUI _inventoryUI;

    [Header("HealthBar")] 
    public Slider _healthBar;

    [Header("PauseMenu")] 
    public GameObject _pauseMenu;

    [Header("SettingPanel")] 
    public GameObject _settingPanel;

    public GameObject _killPad;
    

    private PlayerWeaponBehavior _playerWeaponBehavior;
    private PlayerBehavior _playerBehavior;
    private PlayerAttributeSystem _playerAttributeSystem;


    private void OnEnable()
    {
        
    }

    private void Start()
    {
        InitController();
    }

    private GunBehavior _gunBehavior;

    public void InitController()
    {
        if (_player == null)
        {
            _player = GameplayManager._instance._player;
        }
        
        _playerWeaponBehavior = _player.GetComponent<PlayerWeaponBehavior>();
        _playerBehavior = _player.GetComponent<PlayerBehavior>();
        _playerAttributeSystem = _player.GetComponent<PlayerAttributeSystem>();
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

    public void UpdateWeaponData(GameObject currentWeapon)
    {
        _gunBehavior = currentWeapon.GetComponent<GunBehavior>();
    }

    public void ClearSelect()
    {
        _gunBehavior = null;
        _inventoryUI.ClearSelect();
    }

    private void Update()
    {
        ToggleCrossHairVisible();

        if (_gunBehavior != null)
        {
            var ammoStr = $"{_gunBehavior.currentAmmo} / {_gunBehavior.totalAmmo}";
            _ammoText.SetText(ammoStr);
        }

        if (_playerAttributeSystem != null)
        {
            _healthBar.maxValue = _playerAttributeSystem._maxHealth;
            _healthBar.value = _playerAttributeSystem._curHealth;
        }
        
    }

    public void UpdateKillInfo(string killer, string killed)
    {
        _killPad.GetComponent<UIKillPad>().AddKillCard(killer, killed);
    }

    #region ClickEvent

    public void Resume()
    {
        GameplayManager._instance.PauseMenuAction();
    }

    public void BackToMainMenu()
    {
        NetworkManager._afterLeaveRoom += () =>
        {
            SceneManager.LoadScene("NetworkTest");
        };
        
        NetworkManager._instance.LeaveRoom();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowSettingPanel()
    {
        _settingPanel.SetActive(true);
    }

    public void HideSettingPanel()
    {
        _settingPanel.SetActive(false);
    }

    #endregion
}
