using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using Chronomance.Audio;
using static UnityEngine.ParticleSystem;
using static UnityEngine.UI.Image;

public class ConeOfCold : Spell
{
    private PlayerCharacter player;
    private ObjectPool<Spell> spellPool;
    private float elapsedTime;
    protected ParticleSystem particleInstance;
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
    public override void Initialize(SpellData data, GameObject playerObj, bool isFacingRight)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isFacingRight ? 1f : -1f);
        transform.localScale = scale;

        spellData = data;

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

        elapsedTime = 0f;
        transform.position = player.transform.position;
        gameObject.SetActive(true);
    }


    public override void SetPool(ObjectPool<Spell> pool)
    {
        spellPool = pool;
    }
}

