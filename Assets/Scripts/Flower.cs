using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField]
    [Range(0, 4)]
    public int typeIndex;

    [SerializeField]
    [Range(1, 5)]
    public int stage;

    [SerializeField]
    public Sprite[] stageSprites; // 5 stage sprites

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetStage(int newStage)
    {
        stage = Mathf.Clamp(newStage, 1, 5);
        spriteRenderer.sprite = stageSprites[stage - 1];
    }

    public void Initialize(int type, int startStage, Vector3 position)
    {
        typeIndex = type;
        transform.position = position;
        SetStage(startStage);
    }

    public FlowerData ToData()
    {
        return new FlowerData(typeIndex, stage, transform.position);
    }
}
