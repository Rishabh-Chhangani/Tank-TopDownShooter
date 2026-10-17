using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMarkSpawner : MonoBehaviour
{
    private Vector2 lastPosition;
    public float trackDistance = 0.2f;
    private float trackDistanceSqr;
    public GameObject trackMarkPrefab;
    public int objectPoolSize = 50;

    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    private void Start()
    {
        lastPosition = transform.position;
        trackDistanceSqr = trackDistance * trackDistance;
        objectPool.Initialize(trackMarkPrefab, objectPoolSize);
    }

    private void Update()
    {
        if (((Vector2)transform.position - lastPosition).sqrMagnitude >= trackDistanceSqr)
        {
            lastPosition = transform.position;
            var tracks = objectPool.CreateObject();
            tracks.transform.position = transform.position;
            tracks.transform.rotation = transform.rotation;
        }
    }

}
