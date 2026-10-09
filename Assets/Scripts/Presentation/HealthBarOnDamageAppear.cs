using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarOnDamageAppear : MonoBehaviour
{
    [SerializeField]
   private Damageable damageable;
    [SerializeField]
    private GameObject healthCanvasGameObject;

    private void OnEnable()
    {
        damageable.OnDamaged += ShowHealthBar;
    }

    private void OnDisable()
    {
        damageable.OnDamaged -= ShowHealthBar;
    }

    private void ShowHealthBar()
    {
        
        healthCanvasGameObject.SetActive(true);
    }
}
