using UnityEngine;

[System.Serializable]
public class FlowerData
{
    public int flowerType;
    public int stage;
    public Vector3 position;

    public FlowerData(int flowerType, int stage, Vector3 position)
    {
        this.flowerType = flowerType;
        this.stage = stage;
        this.position = position;
    }
}
