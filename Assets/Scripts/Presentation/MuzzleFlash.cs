using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField]
    private WeaponTurret turret;
    [SerializeField]
    private Animator animator;


    private void Awake()
    {
        if (turret == null)
            turret = GetComponentInParent<WeaponTurret>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (turret != null)
        {
            turret.OnShoot += MuzzleFlashEffect;
        }
    }

    private void OnDisable()
    {
        if (turret != null)
        {
            turret.OnShoot -= MuzzleFlashEffect;
        }
    }

    private void MuzzleFlashEffect()
    {
        if (animator != null)
        {
            animator.enabled = true;
            animator.Play("MuzzleFlash Animation", 0, 0f);
        }
    }
}
