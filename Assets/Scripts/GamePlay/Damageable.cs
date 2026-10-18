using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int currentHealth = 0;

    public int maxHealth = 100;

    [SerializeField] private GameObject entityToDestroy;
    private bool isDead = false;

    public event Action OnDeath;
    public event Action OnDamaged;
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        if (currentHealth == 0)
            currentHealth = maxHealth;

        if (entityToDestroy == null)
        {
            var tankController = GetComponentInParent<TankController>();
            entityToDestroy = tankController != null ? tankController.gameObject : gameObject;
        }
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        

        set
        {
            currentHealth = value;

            currentHealth = Mathf.Clamp(currentHealth,0,maxHealth);

            OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
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