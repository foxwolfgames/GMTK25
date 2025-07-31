using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float cooldown = 1f;
    public abstract void Cast(GameObject user);
}
