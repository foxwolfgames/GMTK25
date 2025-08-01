using UnityEngine;

public abstract class Spell : ScriptableObject
{
    [Header("Values")]
    public string spellName;
    public float cooldown = 1f;
    public float damage = 10f;
    public float rangeX = 5f;
    public float rangey = 3f;

    [Header("References")]
    public ParticleSystem particle;

    [Header("Instance")]
    protected ParticleSystem particleInstance;

    [Header("Debug")]
    protected LineRenderer debugBox;
    public bool showDebugBox = true;
    
    public abstract void Cast(GameObject player);
    protected void DrawDebugBox(Vector2 center, Vector2 size)
    {
        if (!showDebugBox) return;

        if (!debugBox)
        {
            GameObject lineObj = new GameObject("SpellDebugBox");
            debugBox = lineObj.AddComponent<LineRenderer>();
            debugBox.positionCount = 5;
            debugBox.startWidth = 0.05f;
            debugBox.endWidth = 0.05f;
            debugBox.material = new Material(Shader.Find("Sprites/Default"));
            debugBox.loop = false;
            debugBox.useWorldSpace = true;
        }

        Vector3 topLeft = center + new Vector2(-size.x / 2, size.y / 2);
        Vector3 topRight = center + new Vector2(size.x / 2, size.y / 2);
        Vector3 bottomRight = center + new Vector2(size.x / 2, -size.y / 2);
        Vector3 bottomLeft = center + new Vector2(-size.x / 2, -size.y / 2);

        debugBox.SetPositions(new Vector3[]
        {
            topLeft, topRight, bottomRight, bottomLeft, topLeft
        });

        debugBox.enabled = true;
    }
}
