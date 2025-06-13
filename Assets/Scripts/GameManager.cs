using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameData GameData { get; private set; } = new();
    public bool IsDebugMode { get; private set; } = false;
    public bool IsSleeping { get; private set; } = false;

    public List<GameObject> flowersTypePrefabs;
    public Transform flowerParent;
    public Collider2D flowerSpawnArea;

    private List<FlowerData> flowers = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void debug_IncreaseSleepTime(int debugAddSeconds)
    {
        if (!IsSleeping)
            return;

        Debug.Log($"Increasing sleep time by {debugAddSeconds} seconds");
        GameData.IncreaseCurrentSleepTime(debugAddSeconds);
    }

    void Update()
    {
        if (!IsSleeping)
            return;

        GameData.IncreaseCurrentSleepTime(Time.deltaTime);
    }

    public void StartSleepSession()
    {
        GameData.StartSleepSession();
        IsSleeping = true;
    }

    public void ToggleDebugMode()
    {
        IsDebugMode = !IsDebugMode;
        Debug.Log($"Debug mode is now {(IsDebugMode ? "enabled" : "disabled")}");
    }

    public void StopSleepSession() => IsSleeping = false;

    public void EndSleepSession()
    {
        GameData.EndSleepSession();
        UpdateFlowers();
        SaveProgress();
    }

    void UpdateFlowers()
    {
        int newFlowers = GameData.GetAmountOfFlowers();
        for (int i = 0; i < newFlowers; i++)
        {
            AddNewRandomFlower();
        }
    }

    public void SaveProgress()
    {
        // Save game data to persistent storage (e.g., PlayerPrefs, file, etc.)
        // This is a placeholder for actual saving logic
        Debug.Log("Saving game progress...");
        GameData.SaveProgress();
        PlayerPrefs.SetInt("flowerCount", flowers.Count);
        for (int i = 0; i < flowers.Count; i++)
        {
            var data = flowers[i];
            PlayerPrefs.SetInt($"flower_{i}_type", data.flowerType);
            PlayerPrefs.SetInt($"flower_{i}_stage", data.stage);
            PlayerPrefs.SetFloat($"flower_{i}_x", data.position.x);
            PlayerPrefs.SetFloat($"flower_{i}_y", data.position.y);
            PlayerPrefs.SetFloat($"flower_{i}_z", data.position.z);
        }
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        // Load game data from persistent storage (e.g., PlayerPrefs, file, etc.)
        // This is a placeholder for actual loading logic
        Debug.Log("Loading game progress...");
        GameData.LoadProgress();
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
        GameData.Reset();
        flowers.Clear();
        foreach (Transform child in flowerParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Flower InitializeFlower(int type, int stage, Vector3 position)
    {
        Debug.Log("Creating Flower: " + type + " at position: " + position);
        GameObject flowerObj = Instantiate(
            flowersTypePrefabs[type],
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
        int flowerTypeIndex = UnityEngine.Random.Range(0, flowersTypePrefabs.Count);
        Vector3 position = GetRandomPointInCollider(flowerSpawnArea);
        Flower flower = InitializeFlower(flowerTypeIndex, 0, position);
        flowers.Add(flower.ToData());
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
        // clear the current list and build it again from the scene
        flowers.Clear();
        foreach (Transform child in flowerParent)
        {
            if (!child.TryGetComponent<Flower>(out var flowerComponent))
            {
                Debug.LogWarning("Child does not have a Flower component, skipping.");
                continue;
            }
            if (flowerComponent.stage >= 5)
            {
                Debug.LogWarning("Flower is already at max stage, skipping.");
                continue;
            }
            Debug.Log(
                $"Increasing stage for flower of type {flowerComponent.flowerType} from {flowerComponent.stage} to {flowerComponent.stage + 1}"
            );
            flowerComponent.SetNextStage();
            // Update the flower data
            flowers.Add(flowerComponent.ToData());
        }
        SaveProgress();
    }

    public string buildSummaryText()
    {
        string currentTime = Util.GetFormattedTime(GameData.LastGameSleepTime, true);
        string totalTime = Util.GetFormattedTime(GameData.TotalSleepTime, true);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("סיכום שינה:");
        sb.AppendLine($"זמן שינה שנצבר: {currentTime}");
        sb.AppendLine($"סה״כ זמן שינה: {totalTime}");
        sb.AppendLine($"אסימונים שנצברו: {Util.reverseString(GameData.LastGameTokens.ToString())}");
        sb.AppendLine($"סה״כ אסימונים: {Util.reverseString(GameData.TotalTokens.ToString())}");
        sb.AppendLine($"פרחים שנצברו: {Util.reverseString(GameData.GetAmountOfFlowers().ToString())}");
        sb.AppendLine($"סה״כ פרחים: {Util.reverseString(flowers.Count.ToString())}");

        return sb.ToString();
    }

    public string buildScoreText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"אסימונים: {Util.reverseString(GameData.TotalTokens.ToString())} | ");
        sb.Append($"פרחים: {Util.reverseString(flowers.Count.ToString())} | ");
        sb.Append($"זמן שינה: {Util.GetFormattedTime(GameData.TotalSleepTime, true)}");

        return sb.ToString();
    }
}
