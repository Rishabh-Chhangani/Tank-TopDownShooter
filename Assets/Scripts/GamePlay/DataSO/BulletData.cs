using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuletData", menuName = "Data/BulletData")]
public class BulletData : ScriptableObject
{
    public float speed = 10;
    public int damage = 50;
    public float maxDistance = 100;
}
