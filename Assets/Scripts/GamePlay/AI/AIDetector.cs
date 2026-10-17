using System.Collections;
using UnityEngine;

public class AIDetector : MonoBehaviour
{
    [Header("Detection")]
    [Range(1f, 15f)] [SerializeField] private float viewRadius = 11f;

    [SerializeField]
    private float detectionCheckDelay = 0.1f;

    // Not serialized: target must come only from runtime detection,
    // never from a reference assigned in the Inspector / prefab.
    private Transform target;   

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private LayerMask visibilityLayer;

    [Header("Shooting")]
    [SerializeField] private float shootingRange = 6f;

    [field: SerializeField] public bool TargetVisible { get; private set; }

    public Transform Target
    {
        get => target;

        set
        {
            target = value;
            TargetVisible = false;
        }
    }

    private readonly Collider2D[] overlapResults = new Collider2D[1];
    private readonly RaycastHit2D[] raycastHits = new RaycastHit2D[1];

    public bool IsTargetInShootingRange(Transform shooter)
    {
        if (Target == null || shooter == null)
            return false;

        float sqrDistance = ((Vector2)shooter.position - (Vector2)Target.position).sqrMagnitude;
        return sqrDistance <= shootingRange * shootingRange;
    }

    public float ShootingRange => shootingRange;

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    private void Update()
    {
        if (Target != null)
        {
            TargetVisible = CheckTargetVisible();
        }
    }

    private bool CheckTargetVisible()
    {
        Vector2 direction = Target.position - transform.position;

        int hitCount = Physics2D.RaycastNonAlloc(
            transform.position,
            direction,
            raycastHits,
            viewRadius,
            visibilityLayer);

        if (hitCount > 0 && raycastHits[0].collider != null)
        {
            return (playerLayerMask & (1 << raycastHits[0].collider.gameObject.layer)) != 0;
        }

        return false;
    }

    private void DetectTarget()
    {
        if (Target == null)
        {
            CheckIfPlayerInRange();
        }
        else
        {
            DetectIfOutOfRange();
        }
    }

    private void DetectIfOutOfRange()
    {
        if (Target == null || !Target.gameObject.activeSelf)
        {
            Target = null;
            return;
        }

        float maxRange = viewRadius + 1f;
        if (((Vector2)transform.position - (Vector2)Target.position).sqrMagnitude > maxRange * maxRange)
        {
            Target = null;
        }
    }

    private void CheckIfPlayerInRange()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            viewRadius,
            overlapResults,
            playerLayerMask);

        if (hitCount > 0 && overlapResults[0] != null)
        {
            Target = overlapResults[0].transform;
        }
    }

    private IEnumerator DetectionCoroutine()
    {
        var wait = new WaitForSeconds(detectionCheckDelay);
        while (true)
        {
            DetectTarget();
            yield return wait;
        }
    }

    private void OnDrawGizmos()
    {
        TankController tank =
            GetComponentInParent<TankController>();

        Vector3 rangeOrigin = transform.position;

        if (tank != null)
        {
            rangeOrigin = tank.transform.position;
        }

        // Detection range
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(
            rangeOrigin,
            viewRadius);

        // Shooting range
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            rangeOrigin,
            shootingRange);
    }
}