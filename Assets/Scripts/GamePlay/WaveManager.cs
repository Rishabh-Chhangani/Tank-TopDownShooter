using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int maxNumberOfWaves = 2;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int numberOfWaves = 0;
    private int enemiesAlive = 0;

    private void Start()
    {
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed += EnemyDestroyed;
            StartCoroutine(WaveLoop());
        }
        else
        {
            Debug.LogError("SpawnManager is not assigned to WaveManager.", this);
        }
    }

    private void OnDestroy()
    {
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed -= EnemyDestroyed;
        }
    }

    private IEnumerator WaveLoop()
    {
        while (numberOfWaves < maxNumberOfWaves)
        {
            numberOfWaves++;

            enemiesAlive = spawnManager.GetNumberOfEnemiesToSpawn();

            yield return StartCoroutine(spawnManager.ManageSpawn());

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            if (numberOfWaves < maxNumberOfWaves)
            { 
                spawnManager.ResetEnemySpawnPoints();
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
}