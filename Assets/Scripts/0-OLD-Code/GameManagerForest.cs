using UnityEngine;

public class GameManagerForest : MonoBehaviour
{
    [SerializeField]
    private GameObject treePrefab;

    [SerializeField]
    private int treeCount = 10;

    [SerializeField]
    private Collider2D spawnArea;

    [SerializeField]
    private float spawnPadding = 0.5f; // Padding to keep trees from edges

    void Start()
    {
        Bounds bounds = spawnArea.bounds;
        for (int i = 0; i < treeCount; i++)
        {
            // Pick random position inside visible area
            float x = Random.Range(bounds.min.x + spawnPadding, bounds.max.x - spawnPadding);
            float y = Random.Range(bounds.min.y + spawnPadding, bounds.max.y - spawnPadding);
            Vector2 spawnPos = new Vector2(x, y);

            // Instantiate and assign random stage
            GameObject tree = Instantiate(treePrefab, spawnPos, Quaternion.identity);
            TreeGrowth treeGrowth = tree.GetComponent<TreeGrowth>();
            int randomStage = Random.Range(0, treeGrowth.MaxStage() + 1);
            treeGrowth.SetStage(randomStage);
        }
    }
}
