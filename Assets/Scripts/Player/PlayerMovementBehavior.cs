using Cinemachine;
using Manager;
using Photon.Pun;
using UnityEngine;

public class PlayerMovementBehavior : MonoBehaviourPunCallbacks, IPlayerBehavior
{
    public float movementSpeed;
    public float rotateInterpolate;
    public float mouseRotateInterpolate;
    public Rigidbody _playerRigidbody;
    public PlayerBehavior playerBehavior;
    public GameObject followTarget;

    [Header("SFX")] 
    public AudioClip _footAudioClip;
    public AudioSource _audioSource;

    private Vector3 rawMovement;
    private Vector3 smoothMouseRotation;
    private Camera mainCamera;
    private PlayerController _pc;

    private void FixedUpdate()
    {
        if (!_isInitialize) return;
        
        MoveThePlayer();
        TurnThePlayer();
        LockPlayerAngularVelocity();
    }

    private void LockPlayerAngularVelocity()
    {
        _playerRigidbody.angularVelocity = Vector3.zero;
    }

    private void MoveThePlayer()
    {
        Vector3 movement = rawMovement * (movementSpeed * Time.deltaTime);
        
        if (movement.magnitude > 0 && !_audioSource.isPlaying)
        {
            // _footAudioSource.Stop();
            _audioSource.clip = _footAudioClip;
            _audioSource.Play();
        }
        _playerRigidbody.MovePosition(transform.position + movement);
    }

    private CinemachineStateDrivenCamera _sdc;
    private void TurnThePlayer()
    {
        if (playerBehavior._playerState == PlayerState.Combat)
        {
            transform.Rotate(0, smoothMouseRotation.x, 0);
            
            followTarget.transform.Rotate(-smoothMouseRotation.y, 0, 0);

            return;
        }

        if (photonView.IsMine)
        {
            _sdc.transform.Rotate(0, smoothMouseRotation.x, 0);
        }

        if (rawMovement.magnitude > 0.1f && playerBehavior._playerState == PlayerState.Normal)
        {
            Quaternion rotation = Quaternion.Slerp(_playerRigidbody.transform.rotation,
                Quaternion.LookRotation(rawMovement), rotateInterpolate);
            
            // Debug.Log($"CameraDirection {CameraDirection(rawMovement)}, Forward {mainCamera.transform.forward}");
            
            _playerRigidbody.MoveRotation(rotation);
        }
    }

    public void UpdateMovementData(Vector3 movementDirection)
    {
        rawMovement = CameraDirection(movementDirection);
    }

    public void UpdateMouseRotateData(Vector3 mouseInput)
    {
        mouseInput *= Time.deltaTime;
        smoothMouseRotation = Vector3.Lerp(smoothMouseRotation, mouseInput, mouseRotateInterpolate);
    }

    private Vector3 oldForward;
    private Vector3 oldRight;
    private Vector3 CameraDirection(Vector3 movementDirection)
    {
        if (_pc.isFreeCamera)
        {
            return oldForward * movementDirection.z + oldRight * movementDirection.x;
        }
        
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        
        oldForward = cameraForward;
        oldRight = cameraRight;

        return cameraForward * movementDirection.z + cameraRight * movementDirection.x;
    }

    private bool _isInitialize;
    public void Initialize()
    {
        playerBehavior = gameObject.GetComponent<PlayerBehavior>();
        mainCamera = CameraManager._instance.GetGameplayCamera();
        _pc = GetComponent<PlayerController>();
        _sdc = CameraManager._instance._stateDrivenCamera;

        _isInitialize = true;
    }
}