using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages race logic including lap counting and timing
/// </summary>
public class RaceManager : MonoBehaviour
{
    [SerializeField] private int totalLaps = 3;
    [SerializeField] private Transform[] checkpoints;
    [SerializeField] private Transform finishLine;

    private int currentLap = 0;
    private int nextCheckpointIndex = 0;
    private float lapStartTime = 0f;
    private float bestLapTime = float.MaxValue;
    private List<float> lapTimes = new List<float>();

    private CarController playerCar;
    private bool racingStarted = false;

    private void Start()
    {
        playerCar = FindObjectOfType<CarController>();
    }

    public void StartRace()
    {
        currentLap = 1;
        nextCheckpointIndex = 0;
        lapStartTime = Time.time;
        racingStarted = true;
        lapTimes.Clear();
    }

    private void Update()
    {
        if (!racingStarted || playerCar == null)
            return;

        CheckpointDetection();
    }

    private void CheckpointDetection()
    {
        if (nextCheckpointIndex < checkpoints.Length)
        {
            Transform nextCheckpoint = checkpoints[nextCheckpointIndex];
            float distanceToCheckpoint = Vector3.Distance(playerCar.transform.position, nextCheckpoint.position);

            if (distanceToCheckpoint < 15f)
            {
                PassCheckpoint();
            }
        }
        else if (finishLine != null)
        {
            float distanceToFinish = Vector3.Distance(playerCar.transform.position, finishLine.position);
            if (distanceToFinish < 15f)
            {
                CompleteLap();
            }
        }
    }

    private void PassCheckpoint()
    {
        nextCheckpointIndex++;
    }

    private void CompleteLap()
    {
        float lapTime = Time.time - lapStartTime;
        lapTimes.Add(lapTime);

        if (lapTime < bestLapTime)
            bestLapTime = lapTime;

        if (currentLap >= totalLaps)
        {
            FinishRace();
        }
        else
        {
            currentLap++;
            nextCheckpointIndex = 0;
            lapStartTime = Time.time;
        }
    }

    private void FinishRace()
    {
        racingStarted = false;
        if (GameManager.Instance != null)
            GameManager.Instance.FinishRace();
    }

    public int GetCurrentLap() => currentLap;
    public int GetTotalLaps() => totalLaps;
    public float GetBestLapTime() => bestLapTime;
    public float GetCurrentLapTime() => racingStarted ? Time.time - lapStartTime : 0f;
    public List<float> GetAllLapTimes() => lapTimes;

    public string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        int milliseconds = Mathf.FloorToInt((timeInSeconds % 1f) * 1000f);
        return $"{minutes:00}:{seconds:00}.{milliseconds:000}";
    }
}