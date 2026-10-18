using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int currentHealth = 0;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject entityToDestroy;
    private bool isDead = false;

    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;
    public bool IsAlive => !isDead;
    public float HealthNormalized => maxHealth > 0 ? (float)currentHealth / maxHealth : 0f;
    public GameObject EntityToDestroy
    {
        get => entityToDestroy;
        set => entityToDestroy = value;
    }

    public event Action OnDeath;
    public event Action OnDamaged;
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        if (currentHealth == 0)
            currentHealth = maxHealth;

        if (entityToDestroy == null)
        {
            var player = GetComponentInParent<PlayerController>();
            if (player != null)
            {
                entityToDestroy = player.gameObject;
            }
            else
            {
                var enemy = GetComponentInParent<DefaultEnemyAI>();
                if (enemy != null)
                {
                    entityToDestroy = enemy.gameObject;
                }
                else
                {
                    entityToDestroy = (transform.parent != null && transform.parent.name != "Enemies")
                        ? transform.parent.gameObject
                        : gameObject;
                }
            }
        }
    }

    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            currentHealth = value;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            OnHealthChanged?.Invoke(HealthNormalized);
        }
    }

    public void TakeDamage(int damagePoints)
    {
        if (isDead)
            return;
        
        CurrentHealth -= damagePoints;

        if (currentHealth <= 0)
            Die();
        else
            OnDamaged?.Invoke();
    }

    public void Die()
    {
        if (isDead)
            return;
        
        isDead = true;
        
        OnDeath?.Invoke();

        if (entityToDestroy != null)
        {
            Destroy(entityToDestroy);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}