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
        Debug.Log("Velocity: " + rb2d.velocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collied" + collision.name);
        Damagable damagable = collision.gameObject.GetComponent<Damagable>();
        OnHit?.Invoke();
        if (damagable != null)
        {
            damagable.TakeDamage(bulletData.damage);
            Debug.Log("Damage Object : "+ damagable.name);
        }
        DisableBullet();
    }
}