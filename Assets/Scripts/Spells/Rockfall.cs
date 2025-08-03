using Chronomance.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum RockState { Rock, RockBreaking1, RockBreaking2 }
public class Rockfall : Spell
{
    [Header("Values")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0, -0.5f);

    [Header("Gravity Settings")]
    [SerializeField] private float fallAcceleration = 110f;
    [SerializeField] private float maxFallSpeed = 40f;
    [SerializeField] private float groundingForce = -1.5f;

    private PlayerCharacter player;
    private Rigidbody2D rb;
    protected ParticleSystem particleInstance;
    private ObjectPool<Spell> spellPool;
    private HashSet<GameObject> hitTargets = new();
    private Vector2 currentVelocity;
    private RockState state = RockState.Rock;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        HandleGravity();
        transform.position += (Vector3)(currentVelocity * Time.fixedDeltaTime);
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

        state = RockState.Rock;
        spellData = data;
        hitTargets.Clear();
        currentVelocity = Vector2.zero;

        Vector3 directionOffset = player.IsFacingRight ? Vector3.right * spellData.rangeX : Vector3.left * spellData.rangeX;
        Vector3 spawnPosition = player.transform.position + directionOffset + Vector3.up * spellData.rangeY;
        transform.position = spawnPosition;

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

        gameObject.SetActive(true);
    }

    private void HandleGravity()
    {
        if (IsGrounded() && currentVelocity.y <= 0f)
        {
            currentVelocity.y = groundingForce;
        }
        else
        {
            currentVelocity.y = Mathf.MoveTowards(currentVelocity.y, -maxFallSpeed, fallAcceleration * Time.fixedDeltaTime);
        }
    }

    private bool IsGrounded()
    {
        Vector2 checkPosition = (Vector2)transform.position + groundCheckOffset;
        return Physics2D.OverlapCircle(checkPosition, groundCheckRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject target = other.gameObject;
        if (hitTargets.Contains(target))
            return;

        hitTargets.Add(target);

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