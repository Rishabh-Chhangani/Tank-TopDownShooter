using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGeneratorRandomPositionUtil : MonoBehaviour
{
    public GameObject objectPrefab;

    public float radius = 0.2f;

    [SerializeField]
    private Bullet bullet;

    private void Awake()
    {
        if (bullet == null)
            bullet = GetComponent<Bullet>();
    }

    private void OnEnable()
    {
        if (bullet != null)
        {
            bullet.OnHit += CreateObject;
        }
    }

    private void OnDisable()
    {
        if (bullet != null)
        {
            bullet.OnHit -= CreateObject;
        }
    }

    protected Vector2 GetRandomPosition()
    {
        return Random.insideUnitCircle * radius + (Vector2)transform.position;
    }

    protected Quaternion Random2DRotation()
    {
        return Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }

    public void CreateObject()
    {
        if (objectPrefab == null)
            return;

        Vector2 position = GetRandomPosition();
        GameObject impactObject = GetObject();
        if (impactObject != null)
        {
            impactObject.transform.position = position;
            impactObject.transform.rotation = Random2DRotation();
        }
    }

    protected virtual GameObject GetObject()
    {
        return Instantiate(objectPrefab);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
