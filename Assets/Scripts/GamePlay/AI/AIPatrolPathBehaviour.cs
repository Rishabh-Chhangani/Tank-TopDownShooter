using System.Collections;
using UnityEngine;

public class AIPatrolPathBehaviour : AIBehaviour
{
    [SerializeField]
    private PatrolPath patrolPath;

    [Range(0.1f, 1f)]
    [SerializeField]
    private float arriveDistance = 1f;

    [SerializeField]
    private float waitTime = 0.5f;

    [SerializeField]
    private bool isWaiting = false;

    private float currentWaitTimer = 0f;

    [SerializeField]
    private Vector2 currentPatrolTarget = Vector2.zero;

    private bool isInitialized = false;

    private int currentIndex = -1;

    private void Awake()
    {
        if (patrolPath == null)
        {
            patrolPath =
                GetComponentInChildren<PatrolPath>();
        }
    }

    public override void PerformAction(
        TankController tank,
        AIDetector aiDetector)
    {
        if (tank == null ||
            patrolPath == null)
        {
            return;
        }

        if (patrolPath.Length < 2)
            return;

        if (isWaiting)
        {
            tank.HandleTankMovement(Vector2.zero);
            currentWaitTimer -= Time.deltaTime;
            if (currentWaitTimer <= 0f)
            {
                PatrolPath.PathPoint nextPathPoint =
                    patrolPath.GetNextPathPoint(currentIndex);

                currentPatrolTarget = nextPathPoint.Position;
                currentIndex = nextPathPoint.Index;
                isWaiting = false;
            }
            return;
        }

        if (!isInitialized)
        {
            PatrolPath.PathPoint currentPathPoint =
                patrolPath.GetClosestPathPoint(
                    tank.transform.position);

            currentIndex =
                currentPathPoint.Index;

            currentPatrolTarget =
                currentPathPoint.Position;

            isInitialized = true;
        }

        float sqrArriveDistance = arriveDistance * arriveDistance;
        if (((Vector2)tank.transform.position - currentPatrolTarget).sqrMagnitude < sqrArriveDistance)
        {
            tank.HandleTankMovement(Vector2.zero);
            isWaiting = true;
            currentWaitTimer = waitTime;
            return;
        }

        Vector2 directionToGo =
            currentPatrolTarget -
            (Vector2)tank.tankMovement.transform.position;

        if (directionToGo.sqrMagnitude <= 0.01f)
        {
            tank.HandleTankMovement(
                Vector2.zero);

            return;
        }

        float dotProduct =
            Vector2.Dot(
                tank.tankMovement.transform.up,
                directionToGo.normalized);

        if (dotProduct < 0.98f)
        {
            Vector3 crossProduct =
                Vector3.Cross(
                    tank.tankMovement.transform.up,
                    directionToGo.normalized);

            int rotationResult =
                crossProduct.z >= 0 ? -1 : 1;

            tank.HandleTankMovement(
                new Vector2(
                    rotationResult,
                    1f));
        }
        else
        {
            tank.HandleTankMovement(
                Vector2.up);
        }
    }
}