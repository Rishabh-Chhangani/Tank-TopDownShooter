using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    
    private List<SpawnPoint> _spawnPoints;

    [SerializeField]
    private GameObject enemyPrefab;
    


    private void Awake()
    {
         _spawnPoints = new List<SpawnPoint>(FindObjectsOfType<SpawnPoint>());
    }

    public void Spawn()
    {
        if(_spawnPoints.Count == 0)
        {
            Debug.LogWarning("No Spawn points have been found check scene");
            return;
        }

        int index = Random.Range(0, _spawnPoints.Count);
        SpawnPoint spawnPoint = _spawnPoints[index];

        Instantiate(
            enemyPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
            );

        

    }

}
