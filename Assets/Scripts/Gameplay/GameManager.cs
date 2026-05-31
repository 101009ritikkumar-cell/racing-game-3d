using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central game manager handling game states and flow
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState { Menu, Racing, Paused, Finished, GameOver }
    private GameState currentState = GameState.Menu;

    [Header("References")]
    [SerializeField] private CarController playerCar;
    [SerializeField] private RaceManager raceManager;
    [SerializeField] private UIManager uiManager;

    [Header("Game Settings")]
    [SerializeField] private float timeToStart = 3f;

    private float gameStartTime = 0f;
    private bool raceStarted = false;

    public GameState CurrentState => currentState;
    public bool RaceStarted => raceStarted;

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
        SetGameState(GameState.Racing);
        StartRace();
    }

    private void Update()
    {
        HandleInput();
        UpdateGameState();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Racing)
                PauseGame();
            else if (currentState == GameState.Paused)
                ResumeGame();
        }
    }

    private void UpdateGameState()
    {
        if (currentState == GameState.Racing)
        {
            if (!raceStarted && Time.time - gameStartTime > timeToStart)
            {
                raceStarted = true;
                if (uiManager != null)
                    uiManager.HideCountdown();
            }
        }
    }

    private void StartRace()
    {
        gameStartTime = Time.time;
        raceStarted = false;
        if (uiManager != null)
            uiManager.ShowCountdown();
        if (raceManager != null)
            raceManager.StartRace();
    }

    public void FinishRace()
    {
        SetGameState(GameState.Finished);
        if (uiManager != null)
            uiManager.ShowFinishScreen();
        Time.timeScale = 0.5f;
    }

    public void PauseGame()
    {
        SetGameState(GameState.Paused);
        Time.timeScale = 0f;
        if (uiManager != null)
            uiManager.ShowPauseMenu();
    }

    public void ResumeGame()
    {
        SetGameState(GameState.Racing);
        Time.timeScale = 1f;
        if (uiManager != null)
            uiManager.HidePauseMenu();
    }

    public void RestartRace()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void SetGameState(GameState newState)
    {
        currentState = newState;
    }
}