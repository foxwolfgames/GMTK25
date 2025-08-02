using Chronomance.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "ConeOfCold", menuName = "Scriptable Objects/Spell")]
public class SpellData : ScriptableObject
{
    [Header("Values")]
    public string spellName;
    public float cooldown = 1f;
    public float damage = 10f;
    public float rangeX = 5f;
    public float rangey = 3f;
    public float duration = 1f;
    public int manaCost = 20;

    [Header("References")]
    public SoundClipData soundClip;
    public ParticleSystem particle;

    [Header("Debug")]
    protected LineRenderer debugBox;
    public bool showDebugBox = true;
    
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
