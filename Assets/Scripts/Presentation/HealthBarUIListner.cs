using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIListner : MonoBehaviour
{
    [SerializeField] private Slider healthBar;

    [SerializeField]
    private Damagable damagable;

    private void OnEnable()
    {

        if (damagable != null)
        {
            damagable.OnHealthChanged += UpdateHealthBar;


        }
    }
    private void Start()
    {
        if (damagable != null)
        {
            UpdateHealthBar(
                (float)damagable.CurrentHealth / damagable.maxHealth
            );

        }
    }


private void OnDisable()
    {
        if (damagable != null)
        {
            damagable.OnHealthChanged -= UpdateHealthBar;

        }
    }   


    private void UpdateHealthBar(float healthPercentage)
    {
        Debug.Log($"UPDATE HEALTH BAR CALLED: {healthPercentage}");

        healthBar.value = healthPercentage;

        Debug.Log($"SLIDER VALUE NOW: {healthBar.value}");
    }
}
