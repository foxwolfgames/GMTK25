using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Layer
    {
        Ground = 3,
        Water = 4,
        Ice = 6,
        Enemy = 8,
        Player = 9,
    }

    public LayerMask enemyMask;
    public LayerMask waterMask;
    public LayerMask groundMask;
    public LayerMask iceMask;

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
    }
}
