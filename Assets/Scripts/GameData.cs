using System;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private const int MINUTES_TO_TOKEN = 10; // 10 minutes = 1 token
    private const int SESSION_TOKENS = 100; // Tokens earned per session
    private const int MINIMUM_SESSION_TIME = 60 * 60; // 60 minutes minimum session time
    private const int SECONDS_PER_FLOWER = 10 * 60; // 10 minutes to win a flower

    public float LastGameSleepTime { get; private set; } = 0f;
    public float TotalSleepTime { get; private set; } = 0f;
    public int TotalTokens { get; private set; } = 0;
    public int LastGameTokens { get; private set; } = 0;
    public DateTime LastGameDate { get; private set; } = DateTime.Now;

    public void Reset()
    {
        LastGameSleepTime = 0f;
        TotalSleepTime = 0f;
        LastGameTokens = 0;
        TotalTokens = 0;
        LastGameDate = DateTime.Now;
    }

    public void IncreaseCurrentSleepTime(float amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Sleep time cannot be negative.");
        }
        LastGameSleepTime += amount;
    }

    public void StartSleepSession()
    {
        LastGameDate = DateTime.Now; // Update last game time to current time
        LastGameSleepTime = 0f; // Reset current sleep time after ending the session
        LastGameTokens = 0;
    }

    public void EndSleepSession()
    {
        TotalSleepTime += LastGameSleepTime;
        // Calculate total tokens based on accumulated sleep time
        LastGameTokens = CalculateLastGameTokens();
        if (LastGameSleepTime > MINIMUM_SESSION_TIME)
        {
            LastGameTokens += SESSION_TOKENS;
        }
        TotalTokens += LastGameTokens;

        Debug.Log(
            $"Session ended: Tokens: {LastGameTokens}, Total Tokens: {TotalTokens}, " +
            $"Sleep Time: {LastGameSleepTime} seconds, Total Sleep Time: {TotalSleepTime} seconds"
        );
    }

    public int CalculateLastGameTokens()
    {
        return Mathf.FloorToInt(LastGameSleepTime / (MINUTES_TO_TOKEN * 60));
    }

    public int GetAmountOfFlowers()
    {
        return Mathf.FloorToInt(LastGameSleepTime / SECONDS_PER_FLOWER);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetFloat("accumulatedSleepTime", TotalSleepTime);
        PlayerPrefs.SetFloat("lastGameSleepTime", LastGameSleepTime);
        PlayerPrefs.SetInt("totalTokens", TotalTokens);
        PlayerPrefs.SetInt("lastGameTokens", LastGameTokens);
        PlayerPrefs.SetString("lastGameDate", LastGameDate.ToString("o")); // ISO 8601 format
    }

    public void LoadProgress()
    {
        TotalSleepTime = PlayerPrefs.GetFloat("accumulatedSleepTime", 0);
        TotalTokens = PlayerPrefs.GetInt("totalTokens", 0);
        LastGameSleepTime = PlayerPrefs.GetFloat("lastGameSleepTime", 0);
        string lastGameDateString = PlayerPrefs.GetString("lastGameDate", DateTime.Now.ToString("o"));
        if (DateTime.TryParse(lastGameDateString, out DateTime parsedDate))
        {
            LastGameDate = parsedDate;
        }
        else
        {
            LastGameDate = DateTime.Now; // Fallback to current time if parsing fails
        }
    }
}
