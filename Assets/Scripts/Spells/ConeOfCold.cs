using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ConeOfCold", menuName = "Scriptable Objects/Spells/ConeOfCold")]
public class ConeOfCold : Spell
{
    public float damage = 10f;
    public float range = 5f;
    public float angle = 45f;
    public LayerMask hitMask;

    protected override void Cast(GameObject user)
    {
        Vector2 origin = user.transform.position;
        Vector2 direction = user.transform.right;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, range, hitMask);

        foreach (var hit in hits)
        {
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

#if UNITY_EDITOR
    public void DrawDebugGizmo(Vector3 origin, Vector3 direction)
    {
        Handles.color = new Color(0, 0.7f, 1f, 0.3f);

        // Draw the cone as a solid arc
        Handles.DrawSolidArc(origin, Vector3.forward, Quaternion.Euler(0, 0, -angle / 2f) * direction, angle, range);

        // Draw edges
        Vector3 leftEdge = origin + Quaternion.Euler(0, 0, -angle / 2f) * direction * range;
        Vector3 rightEdge = origin + Quaternion.Euler(0, 0, angle / 2f) * direction * range;
        Handles.color = Color.cyan;
        Handles.DrawLine(origin, leftEdge);
        Handles.DrawLine(origin, rightEdge);
    }
#endif
}

