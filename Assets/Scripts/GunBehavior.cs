
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehavior : MonoBehaviour
{
    public GameObject projectile;
    public GameObject gunPoint;
    public float interval;
    public bool isAuto = false;
    public int clipSize;
    public int currentAmmo;
    public int totalAmmo;
    
    private float lastFire;
    private InputActionPhase _actionPhase;
    private Camera _gameplayCamera;

    private void OnEnable()
    {
        _gameplayCamera = CameraManager.Instance.GetGameplayCamera();
    }

    public void Fire(InputActionPhase actionPhase)
    {
        _actionPhase = actionPhase;

        if (FireCheck())
        {
            FireAction();
        }
    }

    private void AutoFire()
    {
        if (FireCheck())
        {
            FireAction();
        }
    }

    private bool FireCheck()
    {
        if (currentAmmo <= 0)
        {
            return false;
        }
        
        if (Time.time - lastFire > interval)
        {
            if (_actionPhase == InputActionPhase.Started)
            {
                return true;
            }

            if (_actionPhase == InputActionPhase.Performed && isAuto)
            {
                return true;
            }
        }

        return false;
    }

    private void FireAction()
    {
        lastFire = Time.time;
        var ray = _gameplayCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = _gameplayCamera.transform.position;
        
        Vector3 aimPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * 1000;
        }

        var fireDirection = aimPoint - gunPoint.transform.position;
        Instantiate(projectile, gunPoint.transform.position, Quaternion.LookRotation(fireDirection));

        currentAmmo = math.clamp(currentAmmo, 0, currentAmmo - 1);
    }

    private void Update()
    {
        AutoFire();
    }
}
