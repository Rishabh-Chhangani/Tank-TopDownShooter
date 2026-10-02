using UnityEngine;

public class AIChaseBehaviour : AIBehaviour
{
    [SerializeField]
    private float rotationThreshold = 0.98f;

    public override void PerformAction(
        TankController tank,
        AIDetector aiDetector)
    {
        // FIX 1: If tank, detector, or target is missing, explicitly stop the tank 
        // instead of leaving the last movement input active!
        if (tank == null || aiDetector == null || aiDetector.Target == null)
        {
            if (tank != null)
            {
                tank.HandleTankMovement(Vector2.zero);
            }
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

        Vector2 tankForward = -tank.tankMovement.transform.up;

        float dotProduct =
            Vector2.Dot(
                tankForward,
                directionToTarget);

        if (dotProduct >= rotationThreshold)
        {
            tank.HandleTankMovement(Vector2.up);
            return;
        }

        float crossProduct =
            tankForward.x * directionToTarget.y -
            tankForward.y * directionToTarget.x;

        // FIX 2: Corrected the typo (-crossProduct3f -> -crossProduct * 3f)
        float rotationResult = Mathf.Clamp(-crossProduct * 3f, -1f, 1f);

        tank.HandleTankMovement(
            new Vector2(
                rotationResult,
                1f));
    }
}