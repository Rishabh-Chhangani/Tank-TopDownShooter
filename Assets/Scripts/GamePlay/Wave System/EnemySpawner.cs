using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<SpawnPoint> _allSpawnPoints;
    private List<SpawnPoint> _availableSpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private Transform enemyParent;

    private void Awake()
    {
        if (_allSpawnPoints == null || _allSpawnPoints.Count == 0)
        {
            _allSpawnPoints = new List<SpawnPoint>(
                FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None)
            );
        }

        _availableSpawnPoints = new List<SpawnPoint>(_allSpawnPoints);
    }

    private void Start()
    {
        if (enemyParent == null)
        {
            GameObject enemyParentObject = GameObject.Find("Enemies");
            if (enemyParentObject != null)
            {
                enemyParent = enemyParentObject.transform;
            }
            else
            {
                enemyParent = new GameObject("Enemies").transform;
            }
        }
    }

    public GameObject Spawn()
    {
        if (_availableSpawnPoints.Count == 0)
        {
            if (_allSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No available Spawn points have been found. Check scene.");
                return null;   
            }

            _availableSpawnPoints = new List<SpawnPoint>(_allSpawnPoints);
        }

        int index = Random.Range(0, _availableSpawnPoints.Count);

        SpawnPoint spawnPoint = _availableSpawnPoints[index];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );

        if (enemyParent != null)
        {
            enemy.transform.SetParent(enemyParent);
        }
        
        _availableSpawnPoints.RemoveAt(index);

        return enemy;
    }




    public void SetSpawnPoints()
    {
        _availableSpawnPoints = new List<SpawnPoint>(_allSpawnPoints);
    }
}