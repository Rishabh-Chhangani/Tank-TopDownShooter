using UnityEngine;

public class AIStaticPatrolBehaviour : AIBehaviour
{
    [SerializeField]
    private float patrolDelay = 1f;

    [SerializeField]
    private Vector2 randomDirection = Vector2.zero;

    [SerializeField]
    private float currentPatrolDelay;

    private void Awake()
    {
        randomDirection =
            Random.insideUnitCircle.normalized;

        currentPatrolDelay = patrolDelay;
    }

    public override void PerformAction(
        TankController tank,
        AIDetector aiDetector)
    {
        if (tank == null ||
            tank.aimTurret == null)
        {
            return;
        }

        float angle =
            Vector2.Angle(
                tank.aimTurret.transform.right,
                randomDirection);

        if (currentPatrolDelay <= 0f &&
            angle < 2f)
        {
            randomDirection =
                Random.insideUnitCircle.normalized;

            currentPatrolDelay = patrolDelay;
        }
        else
        {
            if (currentPatrolDelay > 0f)
            {
                currentPatrolDelay -=
                    Time.deltaTime;
            }
            else
            {
                tank.HandleTurretRotation(
                    (Vector2)tank.aimTurret.transform.position +
                    randomDirection);
            }
        }
    }
}