using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactFeedback : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private InstantiateUtli instantiateUtil;

    private void OnEnable()
    {
        bullet.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        bullet.OnHit -= HandleHit;
    }

    private void HandleHit()
    {
        instantiateUtil.InstantiateObject(bullet.transform.position);
    }
}
