using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public BulletData bulletData;

    private Vector2 startPosition;
    private float conquerDistance = 0;

    public event Action OnHit;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        conquerDistance = Vector2.Distance(transform.position, startPosition);
        if(conquerDistance >= bulletData.maxDistance)
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
        rb2d.velocity = transform.up * bulletData.speed;
       
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