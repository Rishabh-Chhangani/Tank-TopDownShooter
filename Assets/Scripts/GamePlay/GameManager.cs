using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private GameObject player;
    private SaveSystem saveSystem;

    public GameObject Player => player;
    public SaveSystem SaveSystem => saveSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += Initialized;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= Initialized;
        }
    }

    private void Initialized(Scene scene, LoadSceneMode sceneMode)
    {
        var playerInput = FindAnyObjectByType<PlayerInputHandler>();
        if (playerInput != null)
        {
            player = playerInput.gameObject;
        }

        saveSystem = FindObjectOfType<SaveSystem>();
        if (player != null && saveSystem != null && saveSystem.LoadedData != null)
        {
            var damageable = player.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                damageable.CurrentHealth = saveSystem.LoadedData.PlayerHealth;
            }
        }
    }

    public void LoadLevel()
    {
        if (saveSystem != null && saveSystem.LoadedData != null)
        {
            SceneManager.LoadScene(saveSystem.LoadedData.SceneIndex);
            return;
        }
        LoadNextLevel();
    }

    public void LoadNextLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void SaveData()
    {
        if (player != null && saveSystem != null)
        {
            var damageable = player.GetComponentInChildren<Damageable>();
            if (damageable != null)
            {
                saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, damageable.CurrentHealth);
            }
        }
    }
}
