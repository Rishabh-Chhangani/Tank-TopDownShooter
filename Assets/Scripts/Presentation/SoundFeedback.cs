using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFeedback : MonoBehaviour
{
    [SerializeField]
    private WeaponTurret turret;
    [SerializeField]
    private AudioSource audioSource;

    private void Awake()
    {
        if (turret == null)
            turret = GetComponentInParent<WeaponTurret>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (turret != null)
        {
            turret.OnShoot += PlayShootSound;
        }
    }

    private void OnDisable()
    {
        if (turret != null)
        {
            turret.OnShoot -= PlayShootSound;
        }
    }

    private void PlayShootSound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
