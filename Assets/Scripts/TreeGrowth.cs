using UnityEngine;

public class TreeGrowth : MonoBehaviour
{
    [SerializeField] private Sprite[] growthStages; // Drag & drop stage sprites in Inspector
    [SerializeField] private int currentStage = 0;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateStage();
    }

    public void SetStage(int stage)
    {
        currentStage = Mathf.Clamp(stage, 0, growthStages.Length - 1);
        UpdateStage();
    }

    private void UpdateStage()
    {
        if (growthStages.Length > 0 && spriteRenderer != null)
        {
            spriteRenderer.sprite = growthStages[currentStage];
        }
    }

    public int GetStage() => currentStage;
    public int MaxStage() => growthStages.Length - 1;
}
