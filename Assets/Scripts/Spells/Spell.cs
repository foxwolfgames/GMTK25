using UnityEngine;

public abstract class Spell : ScriptableObject
{
    public string spellName;
    public float cooldown = 1f;
    public float damage = 10f;
    public float range = 5f;
    public ParticleSystem particle;

    protected ParticleSystem particleInstance;
    public abstract void Cast(GameObject player);
}
