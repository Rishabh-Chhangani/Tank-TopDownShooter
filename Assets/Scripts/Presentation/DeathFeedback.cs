using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DeathFeedback : MonoBehaviour
{
    [SerializeField] private GameObject deathEffect;

    [SerializeField]
    private Damagable damagable;

    

    private void OnEnable()
    {
        if (damagable != null)
        {
            damagable.OnDeath += PlayDeathFeedback;
        }
    }

    private void OnDisable()
    {
        if (damagable != null)
        {
            damagable.OnDeath -= PlayDeathFeedback;
        }
    }

    private void PlayDeathFeedback()
    {

        Debug.Log("PLAYER DEATH FEEDBACK CALLED");

        GameObject effect = Instantiate(
            deathEffect,
            transform.position,
            Quaternion.identity
        );

        Debug.Log("PLAYER DEATH EFFECT: " + effect.name);

        Animator animator = effect.GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogError("PLAYER DEATH EFFECT HAS NO ANIMATOR");
            return;
        }

        Debug.Log("PLAYER ANIMATOR FOUND");
        Debug.Log("Controller: " + animator.runtimeAnimatorController);
        Debug.Log("Enabled: " + animator.enabled);
    }
}

