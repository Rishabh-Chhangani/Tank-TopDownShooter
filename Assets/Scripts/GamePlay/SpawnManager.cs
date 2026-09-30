using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private EnemySpawner enemySpawner;

    private float spawnRate = 0.4f;
    private int numberOfEnemiesToSpawn = 0;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        if(enemySpawner == null)
        {
            Debug.Log("EnmeySpawner refernce not set");
        }
    }

    private void Start()
    {
        ManageSpawn();
    }

    public void ManageSpawn()
    {
        StartCoroutine(WaitCoroutine());
    }

    


    IEnumerator WaitCoroutine()
    {
        while (numberOfEnemiesToSpawn <= 5)
        {
            enemySpawner.Spawn();
            yield return new WaitForSeconds(spawnRate);
            numberOfEnemiesToSpawn++;
        }
    }
}
