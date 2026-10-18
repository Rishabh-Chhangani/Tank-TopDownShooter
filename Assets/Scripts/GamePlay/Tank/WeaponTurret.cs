using System;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ObjectPool))]
public class WeaponTurret : MonoBehaviour
{
    private TankController tankController;

    private AimTurret aimTurret;

    [SerializeField] private List<Transform> turretBarrels;
    [SerializeField] private TurretData turretData;

    public IReadOnlyList<Transform> TurretBarrels => turretBarrels;
    public TurretData TurretData
    {
        get => turretData;
        set => turretData = value;
    }
    public bool CanShoot => canShoot;
    public float CurrentReloadDelay => currentDelay;

    private bool canShoot = true;
    private Collider2D[] tankColliders;
    private float currentDelay = 0;

    private ObjectPool bulletPool;
    [SerializeField]
    private int bulletPoolCount = 10;

    public event Action OnShoot, OnCanShoot;
    public event Action<float> OnReloading;

    private void Awake()
    {

        tankColliders = GetComponentsInParent<Collider2D>();

        aimTurret = GetComponentInParent<AimTurret>();
        bulletPool = GetComponent<ObjectPool>();


        tankController = GetComponentInParent<TankController>();

        if (tankController == null)
        {
            Debug.LogError("TankController is not found in parent.");
            enabled = false;
            return;
        }

        if (turretData == null || turretData.BulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned.");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        if (turretData != null)
        {
            bulletPool.Initialize(turretData.BulletPrefab, bulletPoolCount);
        }
        OnReloading?.Invoke(currentDelay);
    }

    public void Fire()
    {
        if (canShoot)
        {
            canShoot = false;
            currentDelay = turretData != null ? turretData.ReloadDelay : 0f;

            if (turretBarrels != null)
            {
                foreach (var barrel in turretBarrels)
                {
                    if (barrel == null) continue;

                    GameObject bullet = bulletPool.CreateObject();

                    bullet.transform.position = barrel.position;
                    bullet.transform.rotation = barrel.rotation;

                    bullet.GetComponent<Bullet>().Initialize();

                    if (tankColliders != null)
                    {
                        foreach (var colllider in tankColliders)
                        {
                            if (colllider != null)
                            {
                                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), colllider);
                            }
                        }
                    }
                }
            }

            OnShoot?.Invoke();

            OnReloading?.Invoke(currentDelay);
        }
        else
        {
            OnCanShoot?.Invoke();
        }

    }

    private void Update() => Reload();
    

    private void Reload()
    {
        if (!canShoot)
        {
            currentDelay -= Time.deltaTime;

            float reloadDelay = turretData != null ? turretData.ReloadDelay : 1f;
            OnReloading?.Invoke(reloadDelay > 0f ? currentDelay / reloadDelay : 0f);

            if (currentDelay <= 0)
            {
                currentDelay = 0;
                canShoot = true;

            }
        }
    }
}
