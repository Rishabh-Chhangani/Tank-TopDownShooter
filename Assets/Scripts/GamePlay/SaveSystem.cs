using System;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private string playerHealthKey = "PlayerHealth";
    [SerializeField] private string sceneKey = "SceneIndex";
    [SerializeField] private string savePresentKey = "SavePresent";

    public string PlayerHealthKey => playerHealthKey;
    public string SceneKey => sceneKey;
    public string SavePresentKey => savePresentKey;

    public LoadedData LoadedData { get; private set; }

    public event Action<bool> OnDataLoadedResult;

    public static SaveSystem Instance { get; private set; }

    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
    public int PlayerHealth { get; set; } = -1;
    public int SceneIndex { get; set; } = -1;

    public int playerHealth { get => PlayerHealth; set => PlayerHealth = value; }
    public int sceneIndex { get => SceneIndex; set => SceneIndex = value; }
}