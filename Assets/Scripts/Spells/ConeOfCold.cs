using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ConeOfCold", menuName = "Scriptable Objects/Spells/ConeOfCold")]
public class ConeOfCold : Spell
{
    public float damage = 10f;
    public float range = 5f;
    public float angle = 45f;
    public LayerMask hitMask;

    public override void Cast(GameObject player)
    {
        Debug.Log("Cast Cone");
        Vector2 origin = player.transform.position;
        Vector2 direction = player.GetComponent<PlayerMovement>().Direction;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, range, hitMask);

        foreach (var hit in hits)
        {
            Debug.Log($"Detected object in range: {hit.name}");
            Vector2 toTarget = (Vector2)hit.transform.position - origin;
            float angleToTarget = Vector2.Angle(direction, toTarget);

            if (angleToTarget <= angle / 2f)
            {
                if (hit.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(damage);
                }
            }
        }
    }
}

