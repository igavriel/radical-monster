using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public List<GameObject> flowerPrefabs;
    public Transform flowerParent;
    public Collider2D flowerSpawnArea;

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
    }

    public void StopSleepSession() => isSleeping = false;

    public void EndSleepSession()
    {
        accumulatedSleepTime += currentSleepTime;
        UpdateFlowers();
        SaveProgress();
    }

    public bool IsSleeping()
    {
        return isSleeping;
    }

    void UpdateFlowers()
    {
        int newFlowers = Mathf.FloorToInt(currentSleepTime / flowerWinTimerSeconds);
        for (int i = 0; i < newFlowers; i++)
            AddNewRandomFlower();
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

            Flower flower = InitializeFlower(type, stage, pos);
            flowers.Add(flower.ToData());
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game progress");

        PlayerPrefs.DeleteAll();
        accumulatedSleepTime = 0;
        currentSleepTime = 0;
        flowers.Clear();
        flowerPrefabs.Clear();
        foreach (Transform child in flowerParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Flower InitializeFlower(int type, int stage, Vector3 position)
    {
        Debug.Log("Creating Flower: " + type + " at position: " + position);
        GameObject flowerObj = Instantiate(
            flowerPrefabs[type],
            position,
            Quaternion.identity,
            flowerParent
        );
        Flower flower = flowerObj.GetComponent<Flower>();
        flower.Initialize(type, stage, position);
        return flower;
    }

    public void AddNewRandomFlower()
    {
        int typeIndex = UnityEngine.Random.Range(0, flowerPrefabs.Count);
        Vector3 position = GetRandomPointInCollider(flowerSpawnArea);
        Flower flower = InitializeFlower(typeIndex, 1, position);
        flowers.Add(flower.ToData());
        SaveProgress();
    }

    public void debug_CreateInitialFlowers()
    {
        for (int i = 0; i < 10; i++)
        {
            AddNewRandomFlower();
        }
        SaveProgress();
    }

    private Vector3 GetRandomPointInCollider(Collider2D collider)
    {
        Bounds bounds = collider.bounds;
        Vector3 point;
        do
        {
            point = new Vector3(
                UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                0f
            );
        } while (!collider.OverlapPoint(point));
        return point;
    }

    public void IncreaseAllFlowerStages()
    {
        for (int i = 0; i < flowers.Count; i++)
        {
            var data = flowers[i];
            data.stage = Mathf.Min(data.stage + 1, 5);
        }
        SaveProgress();
    }
}
