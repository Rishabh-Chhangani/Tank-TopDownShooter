using UnityEngine;

public class AIChaseBehaviour : AIBehaviour
{
    [SerializeField]
    private float rotationThreshold = 0.98f;

    public override void PerformAction(
        TankController tank,
        AIDetector aiDetector)
    {
        if (tank == null ||
            aiDetector == null ||
            aiDetector.Target == null)
        {
            return;
        }

        Vector2 directionToTarget =
            (Vector2)aiDetector.Target.position -
            (Vector2)tank.tankMovement.transform.position;

        if (directionToTarget.sqrMagnitude <= 0.01f)
        {
            tank.HandleTankMovement(Vector2.zero);
            return;
        }

        directionToTarget.Normalize();

        Vector2 tankForward =
            tank.tankMovement.transform.up;

        float dotProduct =
            Vector2.Dot(
                tankForward,
                directionToTarget);

        // Tank is facing approximately toward the player.
        if (dotProduct >= rotationThreshold)
        {
            tank.HandleTankMovement(Vector2.up);
            return;
        }

        // Determine which side the player is on.
        float crossProduct =
            tankForward.x * directionToTarget.y -
            tankForward.y * directionToTarget.x;

        int rotationResult =
            crossProduct > 0f ? 1 : -1;

        tank.HandleTankMovement(
            new Vector2(
                rotationResult,
                1f));
    }
}