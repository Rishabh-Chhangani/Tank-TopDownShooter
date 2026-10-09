using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeathFeedback : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;

    [SerializeField]
    private Damageable damageable;

    

    private void OnEnable()
    {
        if (damageable != null)
        {
            damageable.OnDeath += PlayDeathFeedback;
        }
    }

    private void OnDisable()
    {
        if (damageable != null)
        {
            damageable.OnDeath -= PlayDeathFeedback;
        }
    }

    private void PlayDeathFeedback()
    {

        

        GameObject effect = Instantiate(
            deathEffect,
            transform.position,
            Quaternion.identity
        );

        

        Animator animator = effect.GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("PLAYER DEATH EFFECT HAS NO ANIMATOR");
            return;
        }

        
    }
}

