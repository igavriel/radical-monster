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
        Util.AssertObject(spriteRenderer, "Start: SpriteRenderer component not found on Flower object.");
        SetStage(stage);
    }

    public void Initialize(int flowerType, int startStage, Vector3 position)
    {
        Debug.Log($"Initializing flower of type {flowerType} at stage {startStage} at position {position}");
        this.flowerType = flowerType;
        stage = startStage;
        transform.position = position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Util.AssertObject(spriteRenderer, "Initialize: SpriteRenderer component not found on Flower object.");
        SetStage(stage);
    }

    public void SetStage(int newStage)
    {
        if (!spriteRenderer)
        {
            Debug.LogError($"SetStage: SpriteRenderer component not found. Cannot set stage - {gameObject.ToString()}.");
            return;
        }
        Debug.Log($"SetStage: Setting flower stage from {stage} to {newStage}");
        stage = Mathf.Clamp(newStage, 0, MAX_STAGE);
        spriteRenderer.sprite = stageSprites[stage];
    }

    public void SetNextStage()
    {
        SetStage(stage + 1);
    }

    public FlowerData ToData()
    {
        return new FlowerData(flowerType, stage, transform.position);
    }
}
