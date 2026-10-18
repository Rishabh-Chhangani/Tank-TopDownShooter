using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "Data/TurretData")]
public class TurretData : ScriptableObject
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float reloadDelay = 1f;
    [SerializeField] private BulletData bulletData;

    public GameObject BulletPrefab => bulletPrefab;
    public float ReloadDelay => reloadDelay;
    public BulletData BulletData => bulletData;
}
