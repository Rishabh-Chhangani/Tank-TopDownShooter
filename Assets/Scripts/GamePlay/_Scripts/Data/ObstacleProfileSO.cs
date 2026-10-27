using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Profile_", menuName = "PCG/Obstacle Profile")]
public class ObstacleProfileSO : ScriptableObject
{
    [Header("Profile Information")]
    [SerializeField] private string profileName = "Default Profile";

    [Header("Density & Placement")]
    [Tooltip("Minimum and maximum number of obstacle nodes spawned in the arena")]
    [SerializeField] private Vector2Int targetObstacleCount = new Vector2Int(12, 18);

    [Tooltip("Chance (0.0 to 1.0) that a placement node spawns a mini-formation instead of a standalone object")]
    [Range(0f, 1f)]
    [SerializeField] private float formationChance = 0.4f;

    [Header("Allowed Obstacle Pool")]
    [SerializeField] private List<ObstacleDefinitionSO> allowedObstacles = new List<ObstacleDefinitionSO>();

    [Header("Allowed Formation Pool")]
    [SerializeField] private List<FormationDefinitionSO> allowedFormations = new List<FormationDefinitionSO>();

    public string ProfileName => profileName;
    public Vector2Int TargetObstacleCount => targetObstacleCount;
    public float FormationChance => formationChance;
    public IReadOnlyList<ObstacleDefinitionSO> AllowedObstacles => allowedObstacles;
    public IReadOnlyList<FormationDefinitionSO> AllowedFormations => allowedFormations;
}
