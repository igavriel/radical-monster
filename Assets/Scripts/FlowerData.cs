using UnityEngine;

[System.Serializable]
public class FlowerData
{
    public int typeIndex;
    public int stage;
    public Vector3 position;

    public FlowerData(int typeIndex, int stage, Vector3 position)
    {
        this.typeIndex = typeIndex;
        this.stage = stage;
        this.position = position;
    }
}
