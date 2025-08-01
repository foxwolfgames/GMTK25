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
    [SerializeField] private WaterState waterState = WaterState.Water;

    private void Awake()
    {
        tileCollider = GetComponent<Collider2D>();
        tileCollider.isTrigger = true;
    }

    public void Freeze()
    {
        if (waterState == WaterState.Frozen) return;

        waterState = WaterState.Frozen;
        tileCollider.isTrigger = false;

        UpdateVisual();
    }
    public void Thaw()
    {
        if (waterState < 0)
            waterState--;

        if (waterState == WaterState.Water)
            tileCollider.isTrigger = true;

        UpdateVisual();
    }

    private void UpdateVisual()
    {
        Debug.Log($"{gameObject.name} is {waterState}");
    }
}
