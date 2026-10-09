using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private float timeBetweenSpawn = 0.4f;
    [SerializeField] private int numberOfEnemiesToSpawn = 5;

    public event Action OnEnemyDestroyed;

    public int GetNumberOfEnemiesToSpawn()
    {
        return numberOfEnemiesToSpawn;
    }

    public IEnumerator ManageSpawn()
    {
        for (int i = 0; i < numberOfEnemiesToSpawn; i++)
        {
            GameObject enemy = enemySpawner.Spawn();

            if (enemy != null)
            {
                Damageable damageable = enemy.GetComponentInChildren<Damageable>();

                if (damageable != null)
                {
                    damageable.OnDeath += HandleEnemyDeath;
                }
                else
                {
                    Debug.LogError(
                        "Damageable component was not found on spawned enemy."
                    );
                }
            }

            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    private void HandleEnemyDeath()
    {
        OnEnemyDestroyed?.Invoke();
    }

    public void ResetEnemySpawnPoints()

    {
        enemySpawner.ResetSpawnPoints();
    }
}