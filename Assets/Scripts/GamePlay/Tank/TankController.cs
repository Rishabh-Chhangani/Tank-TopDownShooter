using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public TankMovement tankMovement;
    public AimTurret aimTurret;
    public WeaponTurret[] weapon;

    private void Awake()
    {
       

        if (tankMovement == null)
            tankMovement = GetComponent<TankMovement>();

        if (aimTurret == null)
            aimTurret = GetComponentInChildren<AimTurret>();

        if (weapon == null || weapon.Length == 0)
            weapon = GetComponentsInChildren<WeaponTurret>();

    }

    public void HandleTankMovement(Vector2 movementVector)
    {
        if (tankMovement == null)
        {
            Debug.LogError("TankMovement reference is null in TankController. Ensure a TankMovement component exists or assign it in the inspector.");
            return;
        }

        tankMovement.MoveTank(movementVector);
    }

    public void HandleTurretRotation(Vector2 targetPosition)
    {
        aimTurret.RotateTurret(targetPosition);
    }

    public void HandleShoot()
    {
       
        foreach (var w in weapon)
        {
            w.Fire();
        }
    }



}
