using UnityEngine;

public class AIShootBehaviour : AIBehaviour
{
    [Range(1f, 180f)]
    public float fieldOfVisionForShooting = 60f;

    // Added: store the remaining cooldown before next shot
    private float currentShootCooldown = 0f;

    [SerializeField, Min(0f)]
    private float shootCooldown = 1f;

    public override void PerformAction(
      TankController tank,
      AIDetector aiDetector)
    {
        if (tank == null ||
            aiDetector == null ||
            aiDetector.Target == null ||
            tank.aimTurret == null)
        {
            return;
        }

        // Never shoot outside shooting range.
        if (!aiDetector.IsTargetInShootingRange(tank.transform))
            return;

        // Never shoot without line of sight.
        if (!aiDetector.TargetVisible)
            return;

        tank.HandleTurretRotation(
            aiDetector.Target.position);

        bool inFOV = TargetInFOV(
            tank,
            aiDetector);

        if (!inFOV)
            return;

        if (currentShootCooldown > 0f)
            return;

        tank.HandleShoot();

        currentShootCooldown = shootCooldown;
    }

    private bool TargetInFOV(
        TankController tank,
        AIDetector aiDetector)
    {
        if (aiDetector == null ||
            aiDetector.Target == null ||
            tank == null ||
            tank.aimTurret == null)
        {
            return false;
        }

        Vector2 direction =
            aiDetector.Target.position -
            tank.aimTurret.transform.position;

        float angle =
            Vector2.Angle(
                -tank.aimTurret.transform.up,
                direction);

        return angle <
               fieldOfVisionForShooting / 2f;
    }

    private void Update()
    {
        if (currentShootCooldown > 0f)
        {
            currentShootCooldown -= Time.deltaTime;

            if (currentShootCooldown < 0f)
                currentShootCooldown = 0f;
        }
    }
}