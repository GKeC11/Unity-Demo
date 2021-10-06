
using UnityEngine;

public class PlayerAnimatorBehavior : MonoBehaviour, IPlayerBehavior
{
    public enum AnimatorState
    {
        Normal = 0,
        Pistol,
        Rifle,
        Dead,
    }
    
    public Animator playerAnimator;

    private int movementXId;
    private int movementYId;
    private int speedId;
    private int armTriggerID;
    private int disarmTriggerID;
    private int stateID;
    private int dieTriggerID;

    private void Awake()
    {
        
    }

    private void Setup()
    {
        movementXId = Animator.StringToHash("Movement_X");
        movementYId = Animator.StringToHash("Movement_Y");
        speedId = Animator.StringToHash("Speed");
        armTriggerID = Animator.StringToHash("TakeArm");
        disarmTriggerID = Animator.StringToHash("Disarm");
        stateID = Animator.StringToHash("State");
        dieTriggerID = Animator.StringToHash("Die");
    }

    public void UpdateAnimatorState(AnimatorState state)
    {
        // playerAnimator.SetTrigger(isArm ? DisarmTriggerID : ArmTriggerID);
        // isArm = !isArm;
        
        playerAnimator.SetInteger(stateID, (int) state);
        //
        // switch (state)
        // {
        //     case AnimatorState.Normal:
        //         playerAnimator.SetInteger(stateID, (int) AnimatorState.Normal);
        //         break;
        //     case AnimatorState.Pistol:
        //         playerAnimator.SetInteger(stateID, (int) AnimatorState.Pistol);
        //         break;
        //     case AnimatorState.Rifle:
        //         playerAnimator.SetInteger(stateID, (int) AnimatorState.Rifle);
        //         break;
        // }
    }
    
    public void UpdateMovementAnimatorData(Vector3 movementDirection)
    {
        playerAnimator.SetFloat(movementXId, movementDirection.x);
        playerAnimator.SetFloat(movementYId, movementDirection.z);
        playerAnimator.SetFloat(speedId, movementDirection.magnitude);
    }

    public void Initialize()
    {
        Setup();
    }

    public void Die()
    {
        playerAnimator.SetInteger(stateID, (int) AnimatorState.Dead);
        playerAnimator.SetTrigger(dieTriggerID);
    }
}
