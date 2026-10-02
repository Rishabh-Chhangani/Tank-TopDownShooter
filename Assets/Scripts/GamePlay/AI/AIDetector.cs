using System.Collections;
using UnityEngine;

public class AIDetector : MonoBehaviour
{
    [Header("Detection")]
    [Range(1f, 15f)]
    [SerializeField]
    private float viewRadius = 11f;

    [SerializeField]
    private float detectionCheckDelay = 0.1f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private LayerMask visibilityLayer;

    [Header("Shooting")]
    [SerializeField]
    private float shootingRange = 6f;

    [field: SerializeField]
    public bool TargetVisible { get; private set; }

    public Transform Target
    {
        get => target;

        set
        {
            target = value;
            TargetVisible = false;
        }
    }

    public bool IsTargetInShootingRange(Transform shooter)
    {
        if (Target == null || shooter == null)
            return false;

        float distance = Vector2.Distance(
            shooter.position,
            Target.position);

        return distance <= shootingRange;
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
        Vector2 direction =
            Target.position - transform.position;

        RaycastHit2D result = Physics2D.Raycast(
            transform.position,
            direction,
            viewRadius,
            visibilityLayer);

        if (result.collider != null)
        {
            return (playerLayerMask &
                    (1 << result.collider.gameObject.layer)) != 0;
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
        if (Target == null ||
            !Target.gameObject.activeSelf ||
            Vector2.Distance(
                transform.position,
                Target.position) > viewRadius + 1f)
        {
            Target = null;
        }
    }

    private void CheckIfPlayerInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(
            transform.position,
            viewRadius,
            playerLayerMask);

        if (collision != null)
        {
            Target = collision.transform;
        }
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
            DetectTarget();

            yield return new WaitForSeconds(
                detectionCheckDelay);
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