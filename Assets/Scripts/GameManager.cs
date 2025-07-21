using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameData GameData { get; private set; } = new();
    public bool IsDebugMode { get; private set; } = false;
    public bool IsSleeping { get; private set; } = false;

    [Header("Flowers Prefabs")]
    public List<GameObject> flowersTypePrefabs;

    [Header("Front Spawn Settings")]
    public Transform flowerFrontParent;
    public Collider2D flowerFrontSpawnArea;

    [Header("Back Spawn Settings")]
    public Transform flowerBackParent;
    public Collider2D flowerBackSpawnArea;

    // Flower Data Storage
    private List<FlowerData> flowersFront = new();
    private List<FlowerData> flowersBack = new();

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
        SaveFlowerProgress("flowerFront", flowersFront);
        SaveFlowerProgress("flowerBack", flowersBack);
        PlayerPrefs.Save();
    }

    private void SaveFlowerProgress(string prefix, List<FlowerData> flowers)
    {
        // Save the current state of flowers to persistent storage
        PlayerPrefs.SetInt($"{prefix}Count", flowers.Count);
        for (int i = 0; i < flowers.Count; i++)
        {
            var flowerData = flowers[i];
            PlayerPrefs.SetInt($"{prefix}_{i}_type", flowerData.flowerType);
            PlayerPrefs.SetInt($"{prefix}_{i}_stage", flowerData.stage);
            PlayerPrefs.SetFloat($"{prefix}_{i}_x", flowerData.position.x);
            PlayerPrefs.SetFloat($"{prefix}_{i}_y", flowerData.position.y);
            PlayerPrefs.SetFloat($"{prefix}_{i}_z", flowerData.position.z);
        }
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        // Load game data from persistent storage (e.g., PlayerPrefs, file, etc.)
        // This is a placeholder for actual loading logic
        Debug.Log("Loading game progress...");
        GameData.LoadProgress();
        LoadFlowerProgress("flowerFront", flowersFront);
        LoadFlowerProgress("flowerBack", flowersBack);
    }

    private void LoadFlowerProgress(string prefix, List<FlowerData> flowers)
    {
        int count = PlayerPrefs.GetInt($"{prefix}Count", 0);
        flowers.Clear();
        for (int i = 0; i < count; i++)
        {
            int type = PlayerPrefs.GetInt($"{prefix}_{i}_type");
            int stage = PlayerPrefs.GetInt($"{prefix}_{i}_stage");
            float x = PlayerPrefs.GetFloat($"{prefix}_{i}_x");
            float y = PlayerPrefs.GetFloat($"{prefix}_{i}_y");
            float z = PlayerPrefs.GetFloat($"{prefix}_{i}_z");
            Vector3 pos = new Vector3(x, y, z);

            FlowerData flowerData = new FlowerData(type, stage, pos);
            flowers.Add(flowerData);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting game progress");

        PlayerPrefs.DeleteAll();
        GameData.Reset();
        ResetFlowers(flowerFrontParent, flowersFront);
        ResetFlowers(flowerBackParent, flowersBack);
    }

    private void ResetFlowers(Transform flowersParent, List<FlowerData> flowers)
    {
        flowers.Clear();
        foreach (Transform child in flowersParent)
        {
            Destroy(child.gameObject);
        }
    }

    public Flower InitializeFlower(int type, int stage, Vector3 position, Transform flowerParent)
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

        bool isFront = UnityEngine.Random.Range(0, 2) == 0;
        Collider2D flowerSpawnArea = isFront ? flowerFrontSpawnArea : flowerBackSpawnArea;
        Transform flowerParent = isFront ? flowerFrontParent : flowerBackParent;
        List<FlowerData> flowers = isFront ? flowersFront : flowersBack;

        Vector3 position = GetRandomPointInCollider(flowerSpawnArea);
        Flower flower = InitializeFlower(flowerTypeIndex, 0, position, flowerParent);
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
        flowersFront.Clear();
        IncreaseFlowerStages(flowerFrontParent, flowersFront);
        flowersBack.Clear();
        IncreaseFlowerStages(flowerBackParent, flowersBack);
        SaveProgress();
    }

    private void IncreaseFlowerStages(Transform flowerParent, List<FlowerData> flowers)
    {
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
    }

    public string buildSummaryText()
    {
        string currentTime = Util.GetFormattedTime(GameData.LastGameSleepTime, true);
        string totalTime = Util.GetFormattedTime(GameData.TotalSleepTime, true);
        int totalFlowers = flowersFront.Count + flowersBack.Count;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("סיכום שינה:");
        sb.AppendLine($"זמן שינה שנצבר: {currentTime}");
        sb.AppendLine($"סה״כ זמן שינה: {totalTime}");
        sb.AppendLine($"אסימונים שנצברו: {Util.reverseString(GameData.LastGameTokens.ToString())}");
        sb.AppendLine($"סה״כ אסימונים: {Util.reverseString(GameData.TotalTokens.ToString())}");
        sb.AppendLine($"פרחים שנצברו: {Util.reverseString(GameData.GetAmountOfFlowers().ToString())}");
        sb.AppendLine($"סה״כ פרחים: {Util.reverseString(totalFlowers.ToString())}");

        return sb.ToString();
    }

    public void buildScoreText(TMP_Text coins, TMP_Text sleepTime, TMP_Text flowers)
    {
        int totalFlowers = flowersFront.Count + flowersBack.Count;

        coins.text = GameData.TotalTokens.ToString();
        sleepTime.text = Util.GetFormattedTime(GameData.TotalSleepTime, false);
        flowers.text = totalFlowers.ToString();

        //        StringBuilder sb = new StringBuilder();
        //sb.Append($"אסימונים: {Util.reverseString(GameData.TotalTokens.ToString())} | ");
        //sb.Append($"פרחים: {Util.reverseString(totalFlowers.ToString())} | ");
        //sb.Append($"זמן שינה: {Util.GetFormattedTime(GameData.TotalSleepTime, true)}");

    }
}
