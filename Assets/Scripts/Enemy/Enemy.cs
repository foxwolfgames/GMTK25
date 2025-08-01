using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage, Remaining health: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died");
        Destroy(gameObject);
    }
}