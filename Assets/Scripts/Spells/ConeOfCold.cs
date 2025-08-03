using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Chronomance.Audio;
using System.Collections.Generic;

public class ConeOfCold : Spell
{
    private PlayerCharacter player;
    protected ParticleSystem particleInstance;
    private ObjectPool<Spell> spellPool;
    private HashSet<GameObject> hitTargets = new();
    private float elapsedTime;
    private void FixedUpdate()
    {
        if (elapsedTime >= spellData.duration)
        {
            spellPool.Release(this);
            return;
        }

        elapsedTime += Time.fixedDeltaTime;
        transform.position = player.transform.position;
    }
    public override void Initialize(SpellData data, GameObject playerObj)
    {
        if (playerObj.TryGetComponent(out PlayerCharacter pc))
        {
            player = pc;
        }
        else
        {
            Debug.LogError("Initializing spell from a non-player?");
            Destroy(gameObject);
            return;
        }

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (player.IsFacingRight ? 1f : -1f);
        transform.localScale = scale;

        spellData = data;
        elapsedTime = 0f;
        hitTargets.Clear();

        transform.position = player.transform.position;
        gameObject.SetActive(true);

        if (!particleInstance && spellData.particle)
        {
            particleInstance = Instantiate(spellData.particle, transform.position, Quaternion.identity, transform);
            particleInstance.transform.SetParent(gameObject.transform);
        }
        if (particleInstance)
        {
            particleInstance.Clear(true);
            particleInstance.Play(true);
        }

        if (spellData.soundClip)
        {
            AudioSystem.Instance.Play(spellData.soundClip, player.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        if (hitTargets.Contains(target))
            return;

        if (other.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            Debug.Log($"Hit enemy: {other.gameObject.name}");
            damageable.TakeDamage(spellData.damage);
        }
        if (other.TryGetComponent<WaterTile>(out WaterTile waterTile))
        {
            Debug.Log($"Hit Water: {other.gameObject.name}");
            waterTile.Freeze();
        }

    }


    public override void SetPool(ObjectPool<Spell> pool)
    {
        spellPool = pool;
    }
}

