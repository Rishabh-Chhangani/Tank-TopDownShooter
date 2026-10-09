using System;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int currentHealth = 0;

    public int maxHealth = 100;

    private Transform rootEntity;
    private bool isDead = false;

    public event Action OnDeath;
    public event Action OnDamaged;
    public event Action<float> OnHealthChanged;

    private void Awake()
    {
        if (currentHealth == 0)
            currentHealth = maxHealth;

        rootEntity = transform.root;
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

        Destroy(rootEntity.gameObject);
    }
}