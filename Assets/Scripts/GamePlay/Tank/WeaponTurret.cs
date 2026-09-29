using System;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(ObjectPool))]
public class WeaponTurret : MonoBehaviour
{
    
    private TankController tankController;


    private AimTurret aimTurret;

    public List<Transform> turretBarrels;
    public TurretData turretData;


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

        if(tankController == null)
        {
            Debug.LogError("TankController is not found in parent.");
            enabled = false;
            return;
        }

        if (turretData.bulletPrefab == null)
        {
            Debug.LogError("Bullet Prefab is not assigned.");
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        bulletPool.Initialize(turretData.bulletPrefab, bulletPoolCount);
        OnReloading?.Invoke(currentDelay);
    }


  

    public void Fire()
    {

       
        if (canShoot)
        {
           
            canShoot = false;
            currentDelay = turretData.reloadDelay;

            foreach (var barrel in turretBarrels)
            {
                Debug.Log("BARREL COUNT: " + turretBarrels.Count);

                GameObject bullet = bulletPool.CreateObject();

                Debug.Log("FIRING BARREL: " + barrel.name);

                bullet.transform.position = barrel.position;
                bullet.transform.rotation = barrel.rotation;                

                bullet.GetComponent<Bullet>().Initialize();

                foreach (var colllider in tankColliders)
                {
                    Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), colllider);
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

    private void Update()
    {
       
        Reload();
    }

    private void Reload()
    {
        if (!canShoot)
        {
            currentDelay -= Time.deltaTime;

            Debug.Log($"Reloading: {currentDelay}");

            OnReloading?.Invoke(currentDelay / turretData.reloadDelay);

            if (currentDelay <= 0)
            {
                currentDelay = 0;
                canShoot = true;

                Debug.Log("RELOAD COMPLETE - canShoot = TRUE");
            }
        }
    }



}
