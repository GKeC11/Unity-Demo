using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerMovementBehavior playerMovementBehavior;
    public PlayerAnimatorBehavior playerAnimatorBehavior;
    public PlayerWeaponBehavior playerWeaponBehavior;
    public float mouseSensitivity;
    public float smoothSpeed;
    public GameObject followTarget;


    private Vector3 rawInputMovement;
    private Vector3 smoothInputMovement;
    private Vector3 rawInputMouse;
    private Vector3 adjustInputMouse;

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        var inputMovement = ctx.ReadValue<Vector2>();
        rawInputMovement = new Vector3(inputMovement.x, 0, inputMovement.y);
    }

    public void OnMouseRotate(InputAction.CallbackContext ctx)
    {
        rawInputMouse = ctx.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        playerWeaponBehavior.Attack(ctx.phase);
    }

    public void ArmOrDisarm(InputAction.CallbackContext ctx)
    {
        if(ctx.started) playerAnimatorBehavior.UpdateArmAnimatorData();
    }

    public void UseItem(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx);
    }

    private void SmoothInputMovement()
    {
        smoothInputMovement = Vector3.Lerp(smoothInputMovement, rawInputMovement, smoothSpeed);
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
        SmoothInputMovement();
        SmoothMouseInput();
        UpdateMovementBehavior();
        UpdateAnimatorBehavior();
    }
}