using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float cooldown = 1f;

    protected float lastUseTime = -Mathf.Infinity;

    public bool CanCast => Time.time >= lastUseTime + cooldown;

    public void TryCast(GameObject user)
    {
        if (CanCast)
        {
            Cast(user);
            lastUseTime = Time.time;
        }
    }

    protected abstract void Cast(GameObject user);
}
