using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager Instance { get; private set; }

    public Transform WaterTiles;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if (WaterTiles == null)
        {
            Debug.LogWarning("Create water tile parent");
        }
    }
}
