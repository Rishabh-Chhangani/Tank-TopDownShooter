using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public string playerHealthKey = "PlayerHealth";
    public string sceneKey = "SceneIndex";
    public string savePresentKey = "SavePresent";

    public LoadedData LoadedData { get; private set; }

    public event Action<bool> OnDataLoadedResult;

    private bool isInitialized = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (isInitialized)
            return;

        bool result = LoadData();

        OnDataLoadedResult?.Invoke(result);

        isInitialized = true;
    }

    private bool LoadData()
    {
        if (PlayerPrefs.GetInt(savePresentKey) == 1)
        {
            LoadedData = new LoadedData
            {
                playerHealth = PlayerPrefs.GetInt(playerHealthKey),
                sceneIndex = PlayerPrefs.GetInt(sceneKey)
            };

            return true;
        }

        LoadedData = null;

        return false;
    }

    public void SaveData(int sceneIndex, int playerHealth)
    {
        if (LoadedData == null)
            LoadedData = new LoadedData();

        LoadedData.playerHealth = playerHealth;
        LoadedData.sceneIndex = sceneIndex;

        PlayerPrefs.SetInt(playerHealthKey, playerHealth);
        PlayerPrefs.SetInt(sceneKey, sceneIndex);
        PlayerPrefs.SetInt(savePresentKey, 1);

        PlayerPrefs.Save();
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteKey(playerHealthKey);
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(savePresentKey);

        PlayerPrefs.Save();

        LoadedData = null;
    }
}

public class LoadedData
{
    public int playerHealth = -1;
    public int sceneIndex = -1;
}