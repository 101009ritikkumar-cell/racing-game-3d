using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Centralized UI manager handling all game interface elements
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Speed Display")]
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image speedometerNeedle;

    [Header("Race Info")]
    [SerializeField] private TextMeshProUGUI lapText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI bestLapText;

    [Header("Countdown")]
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TextMeshProUGUI countdownText;

    [Header("Finish Screen")]
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private TextMeshProUGUI finishTimeText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseMenuButton;

    [Header("Settings")]
    [SerializeField] private float maxSpeed = 200f;
    private RaceManager raceManager;

    private void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();

        if (restartButton != null)
            restartButton.onClick.AddListener(() => GameManager.Instance.RestartRace());

        if (menuButton != null)
            menuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMenu());

        if (resumeButton != null)
            resumeButton.onClick.AddListener(() => GameManager.Instance.ResumeGame());

        if (pauseMenuButton != null)
            pauseMenuButton.onClick.AddListener(() => GameManager.Instance.ReturnToMenu());

        HideAllPanels();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.CurrentState == GameManager.GameState.Racing)
        {
            UpdateTimerDisplay();
        }
    }

    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
        {
            int speedInt = Mathf.RoundToInt(speed);
            speedText.text = $"{speedInt} km/h";
        }

        if (speedometerNeedle != null)
        {
            float speedRatio = Mathf.Clamp01(speed / (maxSpeed * 3.6f));
            float angle = speedRatio * 270f - 135f;
            speedometerNeedle.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void UpdateRaceInfo(int currentLap, int totalLaps)
    {
        if (lapText != null)
            lapText.text = $"Lap {currentLap}/{totalLaps}";

        if (bestLapText != null && raceManager != null)
        {
            if (raceManager.GetBestLapTime() < float.MaxValue)
                bestLapText.text = $"Best: {raceManager.FormatTime(raceManager.GetBestLapTime())}";
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null && raceManager != null)
        {
            float currentLapTime = raceManager.GetCurrentLapTime();
            timerText.text = raceManager.FormatTime(currentLapTime);
        }
    }

    public void ShowCountdown()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(true);

        StartCoroutine(CountdownCoroutine());
    }

    private System.Collections.IEnumerator CountdownCoroutine()
    {
        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
                countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
            countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        HideCountdown();
    }

    public void HideCountdown()
    {
        if (countdownPanel != null)
            countdownPanel.SetActive(false);
    }

    public void ShowFinishScreen()
    {
        if (finishPanel != null)
        {
            finishPanel.SetActive(true);
            if (finishTimeText != null && raceManager != null)
            {
                var lapTimes = raceManager.GetAllLapTimes();
                float totalTime = lapTimes.Count > 0 ? lapTimes[lapTimes.Count - 1] : 0f;
                finishTimeText.text = $"Time: {raceManager.FormatTime(totalTime)}";
            }
        }
    }

    public void ShowPauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void HidePauseMenu()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    private void HideAllPanels()
    {
        if (countdownPanel != null) countdownPanel.SetActive(false);
        if (finishPanel != null) finishPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }
}