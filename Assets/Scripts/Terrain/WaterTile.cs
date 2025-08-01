using UnityEngine;

public enum WaterState
{
    Water,
    Freezing1,
    Freezing2,
    Frozen,
}
public class WaterTile : MonoBehaviour
{
    private Collider2D tileCollider;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private WaterState waterState = WaterState.Water;

    private void Awake()
    {
        tileCollider = GetComponent<Collider2D>();
        tileCollider.isTrigger = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    public void Freeze()
    {
        if (waterState == WaterState.Frozen) return;

        waterState = WaterState.Frozen;
        tileCollider.isTrigger = false;
        gameObject.layer = (int)GameManager.Layer.Ice;

        UpdateVisual();
    }
    public void Thaw()
    {
        if (waterState < 0)
            waterState--;

        if (waterState == WaterState.Water)
        {
            tileCollider.isTrigger = true;
            gameObject.layer = (int)GameManager.Layer.Water;
        }

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Color color;

        switch (waterState)
        {
            case WaterState.Water:
                color = Color.blue;
                break;
            case WaterState.Freezing1:
                color = new Color(0.4f, 0.6f, 1f);
                break;
            case WaterState.Freezing2:
                color = new Color(0.7f, 0.85f, 1f);
                break;
            case WaterState.Frozen:
                color = Color.white;
                break;
            default:
                color = Color.red; // shouldnt happen
                break;
        }

        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = color;
    }
}
