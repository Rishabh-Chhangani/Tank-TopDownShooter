using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{


    public enum WaveStates
    {
        WavePrep,
        WaveActive,
        WaveCompleted,
        ShopPhase,
        GameOver,
        Victory,
        
    }


    private float wavecountdown = 5f;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int maxNumberOfWaves = 2;
    [SerializeField] private float timeBetweenWaves = 5f;

    private int numberOfWaves = 0;
    private int enemiesAlive = 0;
    private WaveStates currentState;

    public int CurrentWave => numberOfWaves;
    public int MaxNumberOfWaves => maxNumberOfWaves;
    public int EnemiesAlive
    {
        get
        {
            return enemiesAlive;
        }

        set
        {
            enemiesAlive = value;
        }
    }
    public float TimeBetweenWaves => timeBetweenWaves;

    public event Action<WaveStates> OnStateChanged;

    private void Start()
    {
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed += EnemyDestroyed;
          
        }
        SwitchState(WaveStates.WavePrep);
    }

    private void OnDestroy()
    {
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed -= EnemyDestroyed;
        }
    }




    public void SwitchState(WaveStates newState)
    {
        OnExitState(currentState);
        currentState = newState;
        OnEnterState(currentState);
        OnStateChanged?.Invoke(currentState);
    }

    private void OnEnterState(WaveStates currentState)
    {
        switch(currentState)
        {
            case WaveStates.WavePrep:
                numberOfWaves++;
                spawnManager.SetEnemySpawnPoints();
                wavecountdown -= Time.deltaTime;
                break;
            case WaveStates.WaveActive:
                EnemiesAlive = spawnManager.NumberOfEnemiesToSpawn;
                spawnManager.ManageSpawn();
                if(EnemiesAlive <= 0)
                    SwitchState(WaveStates.WaveCompleted);
                break;
            case WaveStates.WaveCompleted:
                if(numberOfWaves >= maxNumberOfWaves)                
                    SwitchState(WaveStates.Victory);
                else
                    SwitchState(WaveStates.ShopPhase);
                break;
            case WaveStates.ShopPhase:
                if(Input.GetKeyDown(KeyCode.Space))
                    SwitchState(WaveStates.WavePrep);
                break;
            case WaveStates.GameOver:
                Time.timeScale = 0f;
                break;
            case WaveStates.Victory:
                Time.timeScale = 0f;
                break;
        }

    }

    private void OnExitState(WaveStates currentState)
    {
        
    }

    private IEnumerator WaveLoop()
    {
        while (numberOfWaves < maxNumberOfWaves)
        {
            numberOfWaves++;

            enemiesAlive = spawnManager.NumberOfEnemiesToSpawn;

            yield return StartCoroutine(spawnManager.ManageSpawn());

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            if (numberOfWaves < maxNumberOfWaves)
            { 
                spawnManager.SetEnemySpawnPoints();
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
}