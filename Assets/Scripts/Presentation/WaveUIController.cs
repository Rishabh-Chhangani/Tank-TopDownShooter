using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WaveUIController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private WaveManager waveManager;

    [Header("HUD Panel")]
    [SerializeField] private GameObject hudPanel;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text enemiesRemainingText;
    [SerializeField] private TMP_Text countdownBannerText;

    [Header("Shop Phase Panel")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text waveCompletedText;
    [SerializeField] private TMP_Text guideText;
    [SerializeField] private Button nextWaveButton;

    [Header("Text Format Templates")]
    [Tooltip("Use {0} for current wave and {1} for total waves")]
    [SerializeField] private string waveTextFormat = "Wave : {0} (Total Waves : {1})";
    [Tooltip("Use {0} for enemies count")]
    [SerializeField] private string enemiesTextFormat = "Enemies : {0}";
    [Tooltip("Use {0} for countdown seconds")]
    [SerializeField] private string countdownTextFormat = "Next Wave: {0}s";
    [Tooltip("Use {0} for completed wave number")]
    [SerializeField] private string waveCompletedFormat = "Wave {0} Completed !";

    [Header("Game Over Panel")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private Button restartButton;

    [Header("Victory Panel")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private Button playAgainButton;

    private void Awake()
    {
        if (waveManager == null)
        {
            waveManager = FindAnyObjectByType<WaveManager>();
        }

        if (nextWaveButton != null)
        {
            nextWaveButton.onClick.AddListener(OnNextWaveClicked);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(OnRestartClicked);
        }
    }

    private void OnEnable()
    {
        if (waveManager != null)
        {
            waveManager.OnStateChanged += HandleStateChanged;
        }
    }

    private void OnDisable()
    {
        if (waveManager != null)
        {
            waveManager.OnStateChanged -= HandleStateChanged;
        }
    }

    private void Start()
    {
        if (waveManager != null)
        {
            HandleStateChanged(waveManager.CurrentState);
        }
    }

    private void Update()
    {
        if (waveManager == null)
            return;

        switch (waveManager.CurrentState)
        {
            case WaveManager.WaveStates.WavePrep:
                UpdateHUD(showCountdown: true);
                break;

            case WaveManager.WaveStates.WaveActive:
                UpdateHUD(showCountdown: false);
                break;
        }
    }

    private void UpdateHUD(bool showCountdown)
    {
        if (waveText != null)
        {
            waveText.text = string.Format(waveTextFormat, waveManager.CurrentWave, waveManager.MaxNumberOfWaves);
        }

        if (enemiesRemainingText != null)
        {
            enemiesRemainingText.text = string.Format(enemiesTextFormat, waveManager.EnemiesAlive);
        }

        if (countdownBannerText != null)
        {
            countdownBannerText.gameObject.SetActive(showCountdown);
            if (showCountdown)
            {
                countdownBannerText.text = string.Format(countdownTextFormat, Mathf.CeilToInt(waveManager.WaveCountDown));
            }
        }
    }

    private void HandleStateChanged(WaveManager.WaveStates state)
    {
        bool isCombat = state == WaveManager.WaveStates.WavePrep || state == WaveManager.WaveStates.WaveActive;
        bool isShop = state == WaveManager.WaveStates.ShopPhase;
        bool isGameOver = state == WaveManager.WaveStates.GameOver;
        bool isVictory = state == WaveManager.WaveStates.Victory;

        if (hudPanel != null) hudPanel.SetActive(isCombat);
        if (shopPanel != null) shopPanel.SetActive(isShop);
        if (gameOverPanel != null) gameOverPanel.SetActive(isGameOver);
        if (victoryPanel != null) victoryPanel.SetActive(isVictory);

        if (isShop)
        {
            if (waveCompletedText != null)
            {
                waveCompletedText.text = string.Format(waveCompletedFormat, waveManager.CurrentWave);
            }

            if (guideText != null)
            {
                guideText.text = "Press [SPACE] or Click to Continue";
            }
        }

        if (isGameOver && gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
        }

        if (isVictory && victoryText != null)
        {
            victoryText.text = "VICTORY!\nAll Waves Cleared!";
        }
    }

    public void OnNextWaveClicked()
    {
        if (waveManager != null && waveManager.CurrentState == WaveManager.WaveStates.ShopPhase)
        {
            waveManager.SwitchState(WaveManager.WaveStates.WavePrep);
        }
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
