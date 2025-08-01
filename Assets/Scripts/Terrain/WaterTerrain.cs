using UnityEngine;

public class WaterTerrain : Terrain
{
    [Header("References")]
    [SerializeField] private GameObject waterTilePrefab;

    private void Start()
    {
        GenerateWater();
    }

    private void GenerateWater()
    {
        if (TerrainManager.Instance == null)
        {
            Debug.LogError("TerrainManager not found");
            return;
        }
        if (waterTilePrefab == null)
        {
            Debug.Log("Water Tile Prefab is null");
            return;
        }
        Debug.Log($"Generating water from {gameObject.name}");

        Vector3 scale = transform.localScale;
        int width = Mathf.RoundToInt(scale.x);
        Vector3 origin = transform.position;

        float topY = origin.y + (scale.y / 2f) - 0.5f;

        for (int x = 0; x < width; x++)
        {
            float xPos = origin.x + x - (width / 2f) + 0.5f;
            Vector3 spawnPos = new Vector3(xPos, topY, 0f);
            GameObject tile = Instantiate(waterTilePrefab, spawnPos, Quaternion.identity, TerrainManager.Instance.WaterTiles);
            //tile.name = $"WaterTile_Top_{x}";
        }
    }
}
