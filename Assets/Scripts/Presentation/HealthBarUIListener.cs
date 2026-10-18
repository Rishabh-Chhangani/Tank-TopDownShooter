using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIListener : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    [SerializeField]
    private Damageable damageable;

    private void Awake()
    {
        if (healthBar == null)
            healthBar = GetComponent<Slider>();

        if (damageable == null)
            damageable = GetComponentInParent<Damageable>();
    }

    private void OnEnable()
    {
        if (damageable != null)
        {
            damageable.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (damageable != null)
        {
            UpdateHealthBar(damageable.HealthNormalized);
        }
    }

    private void OnDisable()
    {
        if (damageable != null)
        {
            damageable.OnHealthChanged -= UpdateHealthBar;
        }
    }   

    private void UpdateHealthBar(float healthPercentage)
    { 
        if (healthBar != null)
        {
            healthBar.value = healthPercentage;
        }
    }
}
