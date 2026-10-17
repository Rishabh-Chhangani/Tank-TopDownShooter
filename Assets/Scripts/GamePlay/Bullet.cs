using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public BulletData bulletData;

    private Vector2 startPosition;
    private float maxDistanceSqr = 0f;

    public event Action OnHit;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (((Vector2)transform.position - startPosition).sqrMagnitude >= maxDistanceSqr)
        {
            DisableBullet();
        }
    }

    private void DisableBullet()
    {
        rb2d.velocity = Vector2.zero;
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        startPosition = transform.position;
        maxDistanceSqr = bulletData != null ? bulletData.maxDistance * bulletData.maxDistance : 0f;
        rb2d.velocity = transform.up * (bulletData != null ? bulletData.speed : 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        OnHit?.Invoke();
        if (damageable != null)
        {
            damageable.TakeDamage(bulletData.damage);
            
        }
        DisableBullet();
    }
}