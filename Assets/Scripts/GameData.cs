using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float CurrentSleepTime { get; private set; } = 0f;
    public float AccumulatedSleepTime { get; private set; } = 0f;
    public DateTime LastGameTime { get; private set; } = DateTime.Now;

    public GameData()
    {
        // Initialize with default values if needed
        CurrentSleepTime = 0f;
        AccumulatedSleepTime = 0f;
        LastGameTime = DateTime.Now;
    }

    public void Reset()
    {
        CurrentSleepTime = 0f;
        AccumulatedSleepTime = 0f;
        LastGameTime = DateTime.Now;
    }

    public void IncreaseCurrentSleepTime(float amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Sleep time cannot be negative.");
        }
        CurrentSleepTime += amount;
    }

    public void StartSleepSession()
    {
        LastGameTime = DateTime.Now; // Update last game time to current time
        CurrentSleepTime = 0f; // Reset current sleep time after ending the session
    }

    public void EndSleepSession()
    {
        AccumulatedSleepTime += CurrentSleepTime;
        CurrentSleepTime = 0f; // Reset current sleep time after ending the session
    }

    public int GetAmountOfFlowers()
    {
        return Mathf.FloorToInt(CurrentSleepTime / GameManager.Instance.flowerWinTimerSeconds);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetFloat("accumulatedSleepTime", AccumulatedSleepTime);
    }

    public void LoadProgress()
    {
        AccumulatedSleepTime = PlayerPrefs.GetFloat("accumulatedSleepTime", 0);
    }
}
