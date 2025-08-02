using UnityEngine;
using UnityEngine.Pool;

public abstract class Spell : MonoBehaviour
{
    public SpellData spellData;
    public abstract void Initialize(SpellData data, GameObject playerObj, bool isFacingRight);
    public abstract void SetPool(ObjectPool<Spell> pool);
}
