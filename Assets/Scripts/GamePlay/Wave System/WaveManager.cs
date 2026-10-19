using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    

    private float _waveCountDown = 5f;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private int maxNumberOfWaves = 2;
    [SerializeField] private float timeBetweenWaves = 5f;

    private Damageable _playerDamageable;
    private Coroutine _spawnCoroutine;
    private int _numberOfWaves = 0;
    private int _enemiesAlive = 0;
    private WaveStates _currentState;

    public int CurrentWave => _numberOfWaves;
    public int MaxNumberOfWaves => maxNumberOfWaves;

    public int EnemiesAlive
    {
        get => _enemiesAlive;

        set => _enemiesAlive = value;
    }

    public float TimeBetweenWaves => timeBetweenWaves;
    public float WaveCountDown => _waveCountDown;
    public WaveStates CurrentState => _currentState;

    public event Action<WaveStates> OnStateChanged;

    private void Start()
    {
        Time.timeScale = 1f;
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed += EnemyDestroyed;
        }
        HookPlayerDeath();
        SwitchState(WaveStates.WavePrep);
        
    }

    private void Update()
    {
        switch (_currentState)
        {
            case WaveStates.WavePrep:
                _waveCountDown -= Time.deltaTime;
                if (_waveCountDown <= 0)
                {
                    SwitchState(WaveStates.WaveActive);
                }
                break;
            case WaveStates.ShopPhase:
                bool spacePressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                                 || Input.GetKeyDown(KeyCode.Space);
                if (spacePressed)
                {
                    SwitchState(WaveStates.WavePrep);
                }
                break;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
        if (spawnManager != null)
        {
            spawnManager.OnEnemyDestroyed -= EnemyDestroyed;
        }

        if (_playerDamageable != null)
        {
            _playerDamageable.OnDeath -= HandlePlayerDeath;
        }
    }


    public void SwitchState(WaveStates newState)
    {
        OnExitState(_currentState);
        _currentState = newState;
        OnEnterState(_currentState);
        OnStateChanged?.Invoke(_currentState);
    }


    private void OnEnterState(WaveStates currentState)
    {
        switch (currentState)
        {
            case WaveStates.WavePrep:
                Time.timeScale = 1f;
                _numberOfWaves++;
                _waveCountDown = timeBetweenWaves;
                if (spawnManager != null)
                {
                    spawnManager.SetEnemySpawnPoints();
                }

                break;
            case WaveStates.WaveActive:
                Time.timeScale = 1f;
                if (spawnManager != null)
                {
                    EnemiesAlive = spawnManager.NumberOfEnemiesToSpawn;
                    _spawnCoroutine = StartCoroutine(spawnManager.ManageSpawn());
                }
                break;
            case WaveStates.WaveCompleted:
                SwitchState(_numberOfWaves >= maxNumberOfWaves ? WaveStates.Victory : WaveStates.ShopPhase);
                break;
            case WaveStates.ShopPhase:
                Time.timeScale = 0f;
                break;
            case WaveStates.GameOver:
            case WaveStates.Victory:
                Time.timeScale = 0f;
                break;
        }
    }

    private void OnExitState(WaveStates currentState)
    {
        if (currentState == WaveStates.ShopPhase)
        {
            Time.timeScale = 1f;
        }

        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = null;
        }
    }

    private void HookPlayerDeath()
    {
        GameObject player = GameManager.Instance != null ? GameManager.Instance.Player : null;
        if(player == null)
        {
            var inputHandler = FindAnyObjectByType<PlayerInputHandler>();
            if (inputHandler != null) player = inputHandler.gameObject;
        }

        if (player != null)
        {
            _playerDamageable = player.GetComponentInChildren<Damageable>();
            if (_playerDamageable != null)
            {
                _playerDamageable.OnDeath += HandlePlayerDeath;
            }
        }

    }

    private void HandlePlayerDeath()
    {
        SwitchState(WaveStates.GameOver);
    }

    private void EnemyDestroyed()
    {
        _enemiesAlive--;
        if (_currentState == WaveStates.WaveActive && _enemiesAlive <= 0)
        {
            SwitchState(WaveStates.WaveCompleted);
        }
    }
}