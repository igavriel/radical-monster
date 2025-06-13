using UnityEngine;

public class Flower : MonoBehaviour
{
    readonly int MAX_STAGE = 4; // Maximum growth stage

    [SerializeField]
    [Range(0, 4)]
    public int flowerType;

    [SerializeField]
    [Range(0, 4)]
    public int stage;

    [SerializeField]
    public Sprite[] stageSprites; // 5 stage sprites

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (stageSprites == null)
        {
            Debug.LogError("Stage sprites not set or incorrect length. Expected 5 sprites.");
            return;
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        Util.AssertObject(spriteRenderer, "SpriteRenderer component not found on Flower object.");
        Debug.Log("Flower Start method called. Initializing flower...");
        SetStage(stage); // Initialize with the current stage
        Debug.Log($"Flower initialized with type {flowerType} at stage {stage}");
    }

    public void SetStage(int newStage)
    {
        Debug.Log($"Setting flower stage from {stage} to {newStage}");
        int nextStage = Mathf.Clamp(newStage, 0, MAX_STAGE);
        if (nextStage == stage)
            return; // Already at this stage

        Util.AssertObject(spriteRenderer, "SpriteRenderer component not found on Flower object. Cannot set stage.");
        spriteRenderer.sprite = stageSprites[stage];
    }

    public void SetNextStage()
    {
        SetStage(stage + 1);
    }

    public void Initialize(int flowerType, int startStage, Vector3 position)
    {
        Debug.Log($"Initializing flower of type {flowerType} at stage {startStage} at position {position}");
        this.flowerType = flowerType;
        this.transform.position = position;
        SetStage(startStage);
    }

    public FlowerData ToData()
    {
        return new FlowerData(flowerType, stage, transform.position);
    }
}
