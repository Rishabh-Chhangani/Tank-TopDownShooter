using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnPoint> _allSpawnPoints;
    [SerializeField] private List<SpawnPoint> _availableSpawnPoints;
    [SerializeField] private GameObject enemyPrefab;

    private Transform enemyParent;

    private void Awake()
    {
        _allSpawnPoints = new List<SpawnPoint>(
            FindObjectsOfType<SpawnPoint>()
        );

        _availableSpawnPoints = new List<SpawnPoint>(_allSpawnPoints);
    }

    private void Start()
    {
        GameObject enemyParentObject = GameObject.Find("Enemies");

        if (enemyParentObject != null)
        {
            enemyParent = enemyParentObject.transform;
        }
        else
        {
            Debug.LogWarning("Enemy Parent not found in scene");
        }
    }

    public GameObject Spawn()
    {
        if (_availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning(
                "No available Spawn points have been found. Check scene."
            );

            return null;
        }

        int index = Random.Range(0, _availableSpawnPoints.Count);

        SpawnPoint spawnPoint = _availableSpawnPoints[index];

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );

        TankController tank = enemy.GetComponentInChildren<TankController>();
        Rigidbody2D rb = tank.GetComponent<Rigidbody2D>();

       

        if (enemyParent != null)
        {
            enemy.transform.SetParent(enemyParent);
        }
        
        _availableSpawnPoints.RemoveAt(index);

        return enemy;
    }


    public void ResetSpawnPoints()
    {
        _availableSpawnPoints = new List<SpawnPoint>(_allSpawnPoints);
    }
}