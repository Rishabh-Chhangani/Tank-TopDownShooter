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
        spawnManager.OnEnemyDestroyed += EnemyDestroyed;

        StartCoroutine(WaveLoop());
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

            yield return new WaitUntil(() => enemiesAlive <= 0);

            

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