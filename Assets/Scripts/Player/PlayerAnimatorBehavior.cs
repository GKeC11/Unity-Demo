using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorBehavior : MonoBehaviour
{
    public Animator playerAnimator;

    private int movementXId;
    private int movementYId;
    private int speedId;
    private int ArmTrigger;
    private int DisarmTrigger;

    private bool isArm = false;

    private void Awake()
    {
        Setup();
    }

    private void Setup()
    {
        movementXId = Animator.StringToHash("Movement_X");
        movementYId = Animator.StringToHash("Movement_Y");
        speedId = Animator.StringToHash("Speed");
        ArmTrigger = Animator.StringToHash("TakeArm");
        DisarmTrigger = Animator.StringToHash("Disarm");
    }

    public void UpdateArmAnimatorData()
    {
        playerAnimator.SetTrigger(isArm ? DisarmTrigger : ArmTrigger);
        isArm = !isArm;
    }
    
    public void UpdateMovementAnimatorData(Vector3 movementDirection)
    {
        playerAnimator.SetFloat(movementXId, movementDirection.x);
        playerAnimator.SetFloat(movementYId, movementDirection.z);
        playerAnimator.SetFloat(speedId, movementDirection.magnitude);
    }
}
