
using System;
using System.Collections;
using ExitGames.Client.Photon;
using Manager;
using Photon.Pun;
using Photon.Realtime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunBehavior : MonoBehaviourPunCallbacks, IOnEventCallback
{

    public enum GunType
    {
        Pistol,
        Rifle,
    }
    
    public GameObject projectile;
    public GameObject gunPoint;
    public float interval;
    public bool isAuto = false;
    public int clipSize;
    public int currentAmmo;
    public int totalAmmo;
    public GunType _type;
    public InventoryItem _ammoType;
    public GameObject _muzzleFlash;
    public GameObject _owner;

    [Header("Attribute")] 
    public int _damage;

    [Header("SFX")] 
    public AudioSource _audioSource;
    
    public AudioClip _gunSFX;

    public AudioClip _reloadSFX;
    
    private float lastFire;
    private InputActionPhase _actionPhase;
    private Camera _gameplayCamera;
    private Inventory _inventory;

    private void Start()
    {
        _gameplayCamera = CameraManager._instance.GetGameplayCamera();
        _inventory = _owner.GetComponent<Inventory>();
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void Fire(InputActionPhase actionPhase)
    {
        _actionPhase = actionPhase;

        if (FireCheck())
        {
            // photonView.RPC("FireAction", RpcTarget.All);
            FireAction();
        }
    }

    private void AutoFire()
    {
        if (FireCheck())
        {
            // photonView.RPC("FireAction", RpcTarget.All);
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
    
    Coroutine muzzleFlashHandler;
    private void FireAction()
    {
        _audioSource.clip = _gunSFX;
        _audioSource.Play();
        
        lastFire = Time.time;
        var ray = _gameplayCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin = _gameplayCamera.transform.position;

        Vector3 aimPoint;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log(hit.collider.gameObject.name);
            aimPoint = hit.point;
        }
        else
        {
            aimPoint = ray.origin + ray.direction * 1000;
        }

        var fireDirection = aimPoint - gunPoint.transform.position;
        
        // 生成子弹
        // var bullet = PhotonNetwork.Instantiate(projectile.name, gunPoint.transform.position, Quaternion.LookRotation(fireDirection));
        var bullet = Instantiate(projectile, gunPoint.transform.position, Quaternion.LookRotation(fireDirection));
        PhotonNetwork.AllocateViewID(bullet.GetComponent<PhotonView>());
        PhotonNetwork.RaiseEvent(EventCodeManager.InstantiateBulletCode,
            new object[] { bullet.GetComponent<PhotonView>().ViewID, photonView.ViewID , bullet.transform.position, bullet.transform.rotation},
            new RaiseEventOptions {Receivers = ReceiverGroup.Others}, SendOptions.SendReliable);
        bullet.GetComponent<ProjectileBehavior>()._spawner = this;

        currentAmmo = math.clamp(currentAmmo, 0, currentAmmo - 1);

        _muzzleFlash.SetActive(true);
        
        if (_muzzleFlash.activeSelf)
        {
            if(muzzleFlashHandler != null) StopCoroutine(muzzleFlashHandler);
            muzzleFlashHandler = StartCoroutine(MuzzleFlashDisappear(0.1f));
        }
    }

    IEnumerator MuzzleFlashDisappear(float time)
    {
        for (float i = 0; i < time; i += 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }

        _muzzleFlash.SetActive(false);
    }
    
    private void Update()
    {
        AutoFire();
        GetInventoryAmmo();
    }

    private void GetInventoryAmmo()
    {
        var slot = _inventory.FindItem(_ammoType);
        if (slot != null)
        {
            totalAmmo = slot.amount;
        }
        else
        {
            totalAmmo = 0;
        }
    }

    public void Reload()
    {
        _audioSource.clip = _reloadSFX;
        _audioSource.Play();
        
        var needAmount = clipSize - currentAmmo;
        var amount = _inventory.RemoveItem(_ammoType, needAmount);
        if (amount > 0)
        {
            currentAmmo += amount;
        }
    }
    
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == EventCodeManager.InstantiateBulletCode)
        {
            var data = (object[]) photonEvent.CustomData;
            if (photonView.ViewID != (int) data[1]) return;
            
            // 生成子弹
            var bullet = Instantiate(projectile, (Vector3) data[2], (Quaternion) data[3]);
            bullet.GetComponent<PhotonView>().ViewID = (int) data[0];
            bullet.GetComponent<ProjectileBehavior>()._spawner = this;

            currentAmmo = math.clamp(currentAmmo, 0, currentAmmo - 1);

            _muzzleFlash.SetActive(true);
            
            if (_muzzleFlash.activeSelf)
            {
                if(muzzleFlashHandler != null) StopCoroutine(muzzleFlashHandler);
                muzzleFlashHandler = StartCoroutine(MuzzleFlashDisappear(0.1f));
            }
        }
    }

}
