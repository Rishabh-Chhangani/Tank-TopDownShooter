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
        turret = GetComponentInParent<WeaponTurret>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        turret.OnShoot += PlayShootSound;
    }

    private void OnDisable()
    {
        turret.OnShoot -= PlayShootSound;
    }

    private void PlayShootSound()
    {
        audioSource.Play();
    }
}
