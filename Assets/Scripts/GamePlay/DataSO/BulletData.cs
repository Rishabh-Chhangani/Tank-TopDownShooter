using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuletData", menuName = "Data/BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 50;
    [SerializeField] private float maxDistance = 100f;

    public float Speed => speed;
    public int Damage => damage;
    public float MaxDistance => maxDistance;
}
