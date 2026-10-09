using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private GameObject player;
    private SaveSystem saveSystem;

    private void Awake()
    {
        SceneManager.sceneLoaded += Initialized;
        DontDestroyOnLoad(gameObject);
    }

    private void Initialized(Scene scene, LoadSceneMode sceneMode)
    {
       
        var playerInput = FindAnyObjectByType<PlayerInputHandler>();
        if (playerInput != null)
        {
            player = playerInput.gameObject;
        }
        saveSystem = FindObjectOfType<SaveSystem>();
        if( player != null && saveSystem.LoadedData != null)
        {
            var damageable = player.GetComponentInChildren<Damageable>();
            damageable.CurrentHealth = saveSystem.LoadedData.playerHealth;

        }
    }

    public void LoadLevel()
    {
        if(saveSystem.LoadedData != null)
        {
            SceneManager.LoadScene(saveSystem.LoadedData.sceneIndex);
            return;
        }
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SaveData()
    {
        if (player != null)
            saveSystem.SaveData(SceneManager.GetActiveScene().buildIndex + 1, player.GetComponentInChildren<Damageable>().CurrentHealth);

    }
}
