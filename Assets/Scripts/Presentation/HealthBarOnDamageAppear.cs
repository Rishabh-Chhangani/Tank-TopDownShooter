using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarOnDamageAppear : MonoBehaviour
{
    [SerializeField]
   private Damagable damagable;
    [SerializeField]
    private GameObject healthCanvasGameObject;

    private void OnEnable()
    {
        damagable.OnDamaged += ShowHealthBar;
    }

    private void OnDisable()
    {
        damagable.OnDamaged -= ShowHealthBar;
    }

    private void ShowHealthBar()
    {
        
        healthCanvasGameObject.SetActive(true);
    }
}
