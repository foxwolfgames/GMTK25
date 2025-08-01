using UnityEditor;
using UnityEngine;
using Chronomance.Audio;

[CreateAssetMenu(fileName = "ConeOfCold", menuName = "Scriptable Objects/Spells/ConeOfCold")]
public class ConeOfCold : Spell
{
    [SerializeField] private SoundClipData soundClip;
    public override void Cast(GameObject player)
    {
        AudioSystem.Instance.Play(soundClip, player.transform);

        GameManager GM = GameManager.Instance;
        LayerMask hitMask = GM.enemyMask | GM.waterMask;
        Debug.Log("Cast Cone of Cold");
        Vector2 origin = player.transform.position;
        Vector2 direction = player.GetComponent<PlayerMovement>().Direction;

        Vector2 boxCenter = origin + new Vector2(direction.x * rangeX * 0.5f, 0f);
        Vector2 boxSize = new Vector2(rangeX, rangey);

        Collider2D[] hits = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0f, hitMask);

        DrawDebugBox(boxCenter, boxSize);

        foreach (var hit in hits)
        {

            if (hit.TryGetComponent(out IDamageable damageable))
            {
                Debug.Log($"Hit Enemy: {hit.name}");
                damageable.TakeDamage(damage);
            }

            if (hit.TryGetComponent(out WaterTile waterTile))
            {
                Debug.Log($"Hit Water: {hit.name}");
                waterTile.Freeze();
            }
        }
        
        // Note that there is one particleInstance being reused, meaning there will be issue if cooldown is less than the duration of the particles

        Transform spellSpawnPoint = player.GetComponent<PlayerCharacter>().SpellSpawnPoint;
        if (particle && spellSpawnPoint)
        {
            if (!particleInstance)
            {
                particleInstance = Instantiate(particle, origin, Quaternion.identity);
                particleInstance.transform.SetParent(spellSpawnPoint);
            }
            particleInstance.transform.rotation = Quaternion.Euler(0f, (direction.x > 0f) ? 0f : 180f, 0f);

            particleInstance.Play();
            particleInstance.Play();
        }

    }
}

