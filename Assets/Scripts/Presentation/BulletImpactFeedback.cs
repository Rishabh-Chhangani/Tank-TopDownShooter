using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpactFeedback : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private InstantiateUtil instantiateUtil;

    private void Awake()
    {
        if (bullet == null)
            bullet = GetComponent<Bullet>();

        if (instantiateUtil == null)
            instantiateUtil = GetComponent<InstantiateUtil>();
    }

    private void OnEnable()
    {
        if (bullet != null)
        {
            bullet.OnHit += HandleHit;
        }
    }

    private void OnDisable()
    {
        if (bullet != null)
        {
            bullet.OnHit -= HandleHit;
        }
    }

    private void HandleHit()
    {
        if (instantiateUtil != null && bullet != null)
        {
            instantiateUtil.InstantiateObject(bullet.transform.position);
        }
    }
}
