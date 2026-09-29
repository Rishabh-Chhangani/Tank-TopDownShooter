using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzelFlash : MonoBehaviour
{
    [SerializeField]
    private WeaponTurret turret;
    [SerializeField]
    private Animator animator;


    private void Awake()
    {
        turret = GetComponentInParent<WeaponTurret>();
        turret.OnShoot += MuzzelFlashEffect;

    }


    private void OnDestroy()
    {
        if (turret != null)
        {
            turret.OnShoot -= MuzzelFlashEffect;
        }
    }

    private void MuzzelFlashEffect()
    {
        Debug.Log("MuzzelFlashEffect");

        animator.enabled = true;
        animator.Play("MuzzelFlash Animation", 0, 0f);

        Debug.Log(
            $"Playing: {animator.GetCurrentAnimatorStateInfo(0).IsName("MuzzelFlash Animation")}"
        );

    }
}
