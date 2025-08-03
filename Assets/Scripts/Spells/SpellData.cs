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
    public float rangeY = 3f;
    public float duration = 1f;
    public int manaCost = 20;

    [Header("References")]
    public SoundClipData soundClip;
    public ParticleSystem particle;
}
