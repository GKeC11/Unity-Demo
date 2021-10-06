using System;
using System.Collections.Generic;
using Cinemachine;
using Manager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public PlayerMovementBehavior playerMovementBehavior;
    public PlayerAnimatorBehavior playerAnimatorBehavior;
    public PlayerWeaponBehavior playerWeaponBehavior;
    public float mouseSensitivity;
    public float smoothInterpolate;
    public bool isFreeCamera;
    public CinemachineStateDrivenCamera _stateDrivenCamera;

    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector3 rawInputMouse;
    private Vector3 adjustInputMouse;

    private PlayerBehavior _playerBehavior;
    private bool isInitializeFinish;

    private void Start()
    {
        if (CameraManager._instance._stateDrivenCamera == null)
        {
            CameraManager._instance._stateDrivenCamera = _stateDrivenCamera;
            _stateDrivenCamera.transform.parent = null;
        }
        else
        {
            Destroy(_stateDrivenCamera.gameObject);
        }

        var behaviors = gameObject.GetComponents<IPlayerBehavior>();
        foreach (var behavior in behaviors)
        {
            behavior.Initialize();
        }

        _playerBehavior = GetComponent<PlayerBehavior>();

        var systems = gameObject.GetComponents<IPlayerSystem>();
        foreach (var system in systems)
        {
            system.Initialize();
        }

        isInitializeFinish = true;
    }


    #region PlayerInputEvent

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;

        if (!photonView.IsMine) return;
        
        var inputMovement = ctx.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnMouseRotate(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;
        
        if (!photonView.IsMine) return;
        
        rawInputMouse = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;
        
        if (!photonView.IsMine) return;
        
        playerWeaponBehavior.Attack(ctx.phase);
        // playerWeaponBehavior.photonView.RPC("Attack", RpcTarget.All, ctx.phase);
    }

    public void Disarm(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;
        
        if (!photonView.IsMine) return;
        
        if (ctx.performed)
        {
            // playerWeaponBehavior.DisArm();
            playerWeaponBehavior.photonView.RPC("DisArm", RpcTarget.All);
        }
    }

    public void UseItem(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;
        
        if (!photonView.IsMine) return;
        
        if (ctx.started)
        {
            var inventoryUI = GameplayManager._instance._gameplayUI.GetComponent<UIController>()._inventoryUI;
            inventoryUI.SelectSlot(int.Parse(ctx.control.displayName));
        }
    }

    public void Reload(InputAction.CallbackContext ctx)
    {
        if (!CheckPlayerState()) return;
        
        if (!photonView.IsMine) return;
        
        if (ctx.started)
        {
            // playerWeaponBehavior.Reload();
            playerWeaponBehavior.photonView.RPC("Reload", RpcTarget.All);
        }
    }

    public void FreeCamera(InputAction.CallbackContext ctx)
    {
        if (!photonView.IsMine) return;
        
        isFreeCamera = true;

        if (ctx.canceled)
        {
            isFreeCamera = false;
        }
    }

    public void DropItem(InputAction.CallbackContext ctx)
    {
        if (!photonView.IsMine) return;
        
        if (ctx.started)
        {
            GameplayManager._instance._player.GetComponent<Inventory>().DropItem();
            playerWeaponBehavior.photonView.RPC("DisArm", RpcTarget.All);
        }
    }

    #endregion


    bool CheckPlayerState()
    {
        if (!isInitializeFinish) return false;
        
        if (_playerBehavior._playerState == PlayerState.Dead) return false;
        
        return true;
    }

    private void SmoothInputMovement()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, smoothInterpolate);
    }

    private void UpdateMovementBehavior()
    {
        playerMovementBehavior.UpdateMovementData(smoothInputMovement);
        playerMovementBehavior.UpdateMouseRotateData(adjustInputMouse);
    }

    private void UpdateAnimatorBehavior()
    {
        playerAnimatorBehavior.UpdateMovementAnimatorData(smoothInputMovement);
    }

    private void SmoothMouseInput()
    {
        adjustInputMouse = rawInputMouse * mouseSensitivity;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        SmoothInputMovement();
        SmoothMouseInput();
        UpdateMovementBehavior();
        UpdateAnimatorBehavior();
    }

    private void OnDestroy()
    {
        if (photonView.IsMine && _stateDrivenCamera != null)
        {
            Destroy(_stateDrivenCamera);
            CameraManager._instance._stateDrivenCamera = null;
        }
    }
}