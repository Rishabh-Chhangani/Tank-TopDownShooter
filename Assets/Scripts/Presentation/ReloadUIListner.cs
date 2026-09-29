using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUIListner : MonoBehaviour
{
    [SerializeField]
    private Slider reloadSlider;

    [SerializeField]
    private WeaponTurret weaponTurret;

    private void OnEnable()
    {
        if (weaponTurret != null)
        {
            weaponTurret.OnReloading += UpdateReloadSlider;
        }
    }

    private void OnDisable()
    {
        if (weaponTurret != null)
        {
            weaponTurret.OnReloading -= UpdateReloadSlider;
        }
    }

    private void UpdateReloadSlider(float reloadPercentage)
    {
        reloadSlider.value = reloadPercentage;
    }

}
