using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float currentSleepTime;
    public float accumulatedSleepTime;
    public DateTime lastGameTime;

    public int flowerWinTimerSeconds = 60; // seconds to win a flower
    public List<FlowerData> flowers = new();

    [Header("Debug")]
    public int debugAddSeconds = 10;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
            Destroy(gameObject);
    }

    private bool isSleepingScene()
    {
        return SceneManager.GetActiveScene().name == "3-2-sleep";
    }

    public void debug_IncreaseSleepTime()
    {
        if (isSleepingScene())
        {
            currentSleepTime += debugAddSeconds;
        }
    }

    void Update()
    {
        if (isSleepingScene())
        {
            currentSleepTime += Time.deltaTime;
        }
    }

    public void StartSleepSession()
    {
        currentSleepTime = 0;
        lastGameTime = DateTime.Now;
    }

    public void EndSleepSession()
    {
        accumulatedSleepTime += currentSleepTime;
        UpdateFlowers();
        SaveProgress();
    }

    void UpdateFlowers()
    {
        int newFlowers = Mathf.FloorToInt(currentSleepTime / flowerWinTimerSeconds);
        for (int i = 0; i < newFlowers; i++)
            flowers.Add(new FlowerData(UnityEngine.Random.Range(0, 5), 1));
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetFloat("accumulatedSleepTime", accumulatedSleepTime);
        PlayerPrefs.SetInt("flowerCount", flowers.Count);
        for (int i = 0; i < flowers.Count; i++)
        {
            PlayerPrefs.SetInt($"flower_{i}_type", flowers[i].typeIndex);
            PlayerPrefs.SetInt($"flower_{i}_stage", flowers[i].stage);
        }
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        accumulatedSleepTime = PlayerPrefs.GetFloat("accumulatedSleepTime", 0);
        int count = PlayerPrefs.GetInt("flowerCount", 0);
        flowers.Clear();
        for (int i = 0; i < count; i++)
        {
            int type = PlayerPrefs.GetInt($"flower_{i}_type");
            int stage = PlayerPrefs.GetInt($"flower_{i}_stage");
            flowers.Add(new FlowerData(type, stage));
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        accumulatedSleepTime = 0;
        currentSleepTime = 0;
        flowers.Clear();
    }
}

[System.Serializable]
public class FlowerData
{
    public int typeIndex;
    public int stage;

    public FlowerData(int typeIndex, int stage)
    {
        this.typeIndex = typeIndex;
        this.stage = stage;
    }
}
