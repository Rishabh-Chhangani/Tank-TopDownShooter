using UnityEngine;

public class DefaultEnemyAI : MonoBehaviour
{
    [Header("AI Behaviours")]
    [SerializeField]
    private AIBehaviour shootBehaviour;

    [SerializeField]
    private AIBehaviour chaseBehaviour;

    [SerializeField]
    private AIBehaviour patrolBehaviour;

    [Header("References")]
    [SerializeField]
    private TankController tank;

    [SerializeField]
    private AIDetector aiDetector;

    private void Awake()
    {
        if (aiDetector == null)
        {
            aiDetector =
                GetComponentInChildren<AIDetector>();
        }

        if (tank == null)
        {
            tank =
                GetComponentInChildren<TankController>();
        }
    }

    private void Update()
    {
        if (tank == null ||
            aiDetector == null)
        {
            return;
        }

        // No target detected.
        // Perform the enemy's normal behaviour.
        if (aiDetector.Target == null)
        {
            if (patrolBehaviour != null)
            {
                patrolBehaviour.PerformAction(
                    tank,
                    aiDetector);
            }

            return;
        }

        // Target detected.
        // Always chase the target.
        if (chaseBehaviour != null)
        {
            chaseBehaviour.PerformAction(
                tank,
                aiDetector);
        }

        // Shooting is independent of chasing.
        // The enemy can move and shoot at the same time.
        if (aiDetector.IsTargetInShootingRange(
                tank.transform) &&
            aiDetector.TargetVisible)
        {
            if (shootBehaviour != null)
            {
                shootBehaviour.PerformAction(
                    tank,
                    aiDetector);
            }
        }
    }
}