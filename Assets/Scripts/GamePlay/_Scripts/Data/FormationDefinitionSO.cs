using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FormationCell
{
    public Vector2Int relativeOffset;
    public ObstacleDefinitionSO obstacle;
}

[CreateAssetMenu(fileName = "Formation_", menuName = "PCG/Formation Definition")]
public class FormationDefinitionSO : ScriptableObject
{
    [SerializeField] private string formationName = "New Formation";
    [SerializeField] private List<FormationCell> elements = new List<FormationCell>();

    [Tooltip("Clearance buffer required around the entire formation bounding box")]
    [SerializeField] private int clearanceBuffer = 2;

    [Range(0.01f, 10f)]
    [SerializeField] private float spawnWeight = 1.0f;

    public string FormationName => formationName;
    public IReadOnlyList<FormationCell> Elements => elements;
    public int ClearanceBuffer => clearanceBuffer;
    public float SpawnWeight => spawnWeight;

    public BoundsInt GetBoundingBox()
    {
        if (elements == null || elements.Count == 0)
            return new BoundsInt(Vector3Int.zero, Vector3Int.one);

        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;

        foreach (var cell in elements)
        {
            minX = Mathf.Min(minX, cell.relativeOffset.x);
            minY = Mathf.Min(minY, cell.relativeOffset.y);
            int footX = cell.obstacle != null ? cell.obstacle.FootprintSize.x : 1;
            int footY = cell.obstacle != null ? cell.obstacle.FootprintSize.y : 1;
            maxX = Mathf.Max(maxX, cell.relativeOffset.x + footX - 1);
            maxY = Mathf.Max(maxY, cell.relativeOffset.y + footY - 1);
        }

        return new BoundsInt(minX, minY, 0, maxX - minX + 1, maxY - minY + 1, 1);
    }
}
