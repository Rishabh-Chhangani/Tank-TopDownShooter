using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShootBehaviour : AIBehaviour
{
    public float fieldOfVisionForShooting = 60;

    public override void PerformAction(TankController tank, AIDetector aiDetector)
    {
        Debug.Log("AI SHOOT ACTION");
        if (tank != null && tank.aimTurret != null)
        {
            tank.HandleTurretRotation(aiDetector.Target.position);
        }
        if (aiDetector == null)
            return;

        Debug.Log($"Target: {aiDetector.Target}");

        if (aiDetector.Target == null)
            return;

        bool inFOV = TargetInFOV(tank, aiDetector);

        Debug.Log($"TargetInFOV: {inFOV}");

        if (inFOV)
        {
            Debug.Log("SHOOTING");
            tank.HandleTankMovement(Vector2.zero);
            tank.HandleShoot();
        }


    }

    
    private bool TargetInFOV(TankController tank, AIDetector aiDetector)
    {
        if (aiDetector == null || aiDetector.Target == null ||
            tank == null || tank.aimTurret == null)
            return false;

        var direction =
            aiDetector.Target.position -
            tank.aimTurret.transform.position;

        float angle = Vector2.Angle(
            -tank.aimTurret.transform.up,
            direction
        );

        Debug.Log($"Target Angle: {angle}");

        return angle < fieldOfVisionForShooting / 2;
    }



}
