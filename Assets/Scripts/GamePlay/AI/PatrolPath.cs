using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();

    public IReadOnlyList<Transform> PatrolPoints => patrolPoints;
    public int Length => patrolPoints != null ? patrolPoints.Count : 0;

    [Header("Gizmo paramenters")]
    [SerializeField] private Color pointsColor = Color.blue;
    [SerializeField] private float pointSize = 1.0f;
    [SerializeField] private Color lineColor = Color.magenta;

    public Color PointsColor => pointsColor;
    public float PointSize => pointSize;
    public Color LineColor => lineColor;

    public struct PathPoint
    {
        public int Index;
        public Vector2 Position;
    }

    public PathPoint GetClosestPathPoint(Vector2 tankPosition)
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            return new PathPoint { Index = -1, Position = tankPosition };
        }

        var minSqrDistance = float.MaxValue;
        var index = 0;
        for (int i = 0; i < patrolPoints.Count; i++)
        {
            if (patrolPoints[i] == null) continue;

            var tempSqrDistance = (tankPosition - (Vector2)patrolPoints[i].position).sqrMagnitude;
            if (tempSqrDistance < minSqrDistance)
            {
                minSqrDistance = tempSqrDistance;
                index = i;
            }
        }

        Vector2 pos = patrolPoints[index] != null ? (Vector2)patrolPoints[index].position : tankPosition;
        return new PathPoint { Index = index, Position = pos };
    }

    public PathPoint GetNextPathPoint(int index)
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            return new PathPoint { Index = -1, Position = Vector2.zero };
        }

        var newIndex = (index + 1) % patrolPoints.Count;
        Vector2 pos = patrolPoints[newIndex] != null ? (Vector2)patrolPoints[newIndex].position : Vector2.zero;
        return new PathPoint { Index = newIndex, Position = pos };
    }
    private void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
            return;
        for (int i = patrolPoints.Count - 1; i >= 0; i--)
        {
            if (patrolPoints[i] == null)
            {
                continue;
            }
            Gizmos.color = pointsColor;
            Gizmos.DrawSphere(patrolPoints[i].position, pointSize);

            if (patrolPoints.Count == 1 || i == 0)
                return;
            Gizmos.color = lineColor;
            Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i - 1].position);

            if(patrolPoints.Count > 2 && i == patrolPoints.Count -1)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
            }
        }
    }

}
