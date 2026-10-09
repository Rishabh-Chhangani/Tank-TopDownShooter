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
        turret = GetComponentInParent<WeaponTurret>();
        turret.OnShoot += MuzzleFlashEffect;

    }


    private void OnDestroy()
    {
        if (turret != null)
        {
            turret.OnShoot -= MuzzleFlashEffect;
        }
    }

    private void MuzzleFlashEffect()
    {
        

        animator.enabled = true;
        animator.Play("MuzzleFlash Animation", 0, 0f);

       

    }
}
