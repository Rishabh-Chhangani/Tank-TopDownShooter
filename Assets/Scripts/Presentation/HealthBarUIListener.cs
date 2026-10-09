using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIListener : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    [SerializeField]
    private Damageable damageable;

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
            UpdateHealthBar(
                (float)damageable.CurrentHealth / damageable.maxHealth
            );

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
        healthBar.value = healthPercentage;
    }
}
