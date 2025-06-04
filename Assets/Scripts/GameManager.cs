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

    private bool isSleeping = false;

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

    public void debug_IncreaseSleepTime(int debugAddSeconds)
    {
        if (isSleeping)
        {
            Debug.Log($"Increasing sleep time by {debugAddSeconds} seconds");
            currentSleepTime += debugAddSeconds;
        }
    }

    void Update()
    {
        if (isSleeping)
        {
            currentSleepTime += Time.deltaTime;
        }
    }

    public void StartSleepSession()
    {
        currentSleepTime = 0;
        lastGameTime = DateTime.Now;
        isSleeping = true;

        SceneManager.LoadScene(Util.SLEEP_SCENE_NAME);
    }

    public void StopSleepSession() => isSleeping = false;

    public void EndSleepSession()
    {
        accumulatedSleepTime += currentSleepTime;
        UpdateFlowers();
        SaveProgress();

        SceneManager.LoadScene(Util.WAKE_SCENE_NAME);
    }

    public bool IsSleeping()
    {
        return isSleeping;
    }

    void UpdateFlowers()
    {
        int newFlowers = Mathf.FloorToInt(currentSleepTime / flowerWinTimerSeconds);
//        for (int i = 0; i < newFlowers; i++)
//            flowers.Add(new FlowerData(UnityEngine.Random.Range(0, 5), 1));
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetFloat("accumulatedSleepTime", accumulatedSleepTime);
        PlayerPrefs.SetInt("flowerCount", flowers.Count);
        for (int i = 0; i < flowers.Count; i++)
        {
            var data = flowers[i];
            PlayerPrefs.SetInt($"flower_{i}_type", data.typeIndex);
            PlayerPrefs.SetInt($"flower_{i}_stage", data.stage);
            PlayerPrefs.SetFloat($"flower_{i}_x", data.position.x);
            PlayerPrefs.SetFloat($"flower_{i}_y", data.position.y);
            PlayerPrefs.SetFloat($"flower_{i}_z", data.position.z);
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
            float x = PlayerPrefs.GetFloat($"flower_{i}_x");
            float y = PlayerPrefs.GetFloat($"flower_{i}_y");
            float z = PlayerPrefs.GetFloat($"flower_{i}_z");
            Vector3 pos = new Vector3(x, y, z);
            //GameObject prefab = flowerPrefabs[type];
            //GameObject flowerObj = Instantiate(prefab, pos, Quaternion.identity, parent);
            //Flower flower = flowerObj.GetComponent<Flower>();
            //flower.Initialize(type, stage, pos);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game progress");

        PlayerPrefs.DeleteAll();
        accumulatedSleepTime = 0;
        currentSleepTime = 0;
        flowers.Clear();
    }
}
