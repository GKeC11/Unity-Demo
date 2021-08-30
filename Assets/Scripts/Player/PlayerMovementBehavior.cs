
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviour
{
    public float movementSpeed;
    public float rotateSpeed;
    public float mouseRotateSpeed;
    public Rigidbody playerRigidbody;
    public PlayerBehavior playerBehavior;
    public GameObject followTarget;

    private Vector3 rawMovement;
    private Vector3 smoothMouseRoration;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = CameraManager.Instance.GetGameplayCamera();
    }

    private void OnEnable()
    {
        playerBehavior = gameObject.GetComponent<PlayerBehavior>();
    }

    private void FixedUpdate()
    {
        MoveThePlayer();
        TurnThePlayer();
        LockPlayerAngularVelocity();
    }

    private void LockPlayerAngularVelocity()
    {
        playerRigidbody.angularVelocity = Vector3.zero;
    }
    
    private void MoveThePlayer()
    {
        Vector3 movement = rawMovement * (movementSpeed * Time.deltaTime);
        playerRigidbody.MovePosition(playerRigidbody.position + movement);
    }

    private void TurnThePlayer()
    {
        if (playerBehavior._playerState == PlayerState.Combat)
        {
            Vector3 rotation = transform.rotation.eulerAngles;
            // rotation.x -= mouseRoration.y;
            rotation.y += smoothMouseRoration.x;
            transform.rotation = Quaternion.Euler(rotation);

            rotation = followTarget.transform.rotation.eulerAngles;
            rotation.x -= smoothMouseRoration.y;
            followTarget.transform.rotation = Quaternion.Euler(rotation);
        }
        if (rawMovement.magnitude > 0.1f && playerBehavior._playerState == PlayerState.Normal)
        {
            Quaternion rotation = Quaternion.Slerp(playerRigidbody.rotation,
                Quaternion.LookRotation(CameraDirection(rawMovement)), rotateSpeed);
            playerRigidbody.MoveRotation(rotation);
        }
    }

    public void UpdateMovementData(Vector3 movementDirection)
    {
        rawMovement = CameraDirection(movementDirection);
    }

    public void UpdateMouseRotateData(Vector3 mouseInput)
    {
        mouseInput *= Time.deltaTime;
        smoothMouseRoration = Vector3.Lerp(smoothMouseRoration, mouseInput, mouseRotateSpeed);
    }

    private Vector3 CameraDirection(Vector3 movementDirection)
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        return cameraForward * movementDirection.z + cameraRight * movementDirection.x;
    }
}