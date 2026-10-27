using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle_", menuName = "PCG/Obstacle Definition")]
public class ObstacleDefinitionSO : ScriptableObject
{
    [Header("Visual & Prefab")]
    [SerializeField] private string obstacleName = "New Obstacle";
    [SerializeField] private GameObject prefab;
    [SerializeField] private ObstacleCategory category = ObstacleCategory.SolidCover;

    [Header("Grid & Footprint")]
    [Tooltip("Dimensions occupied in grid cells (width x height)")]
    [SerializeField] private Vector2Int footprintSize = Vector2Int.one;

    [Tooltip("Number of empty cells required around this obstacle for navigation")]
    [SerializeField] private int clearanceBuffer = 2;

    [Header("Anti-Clustering & Weight")]
    [Tooltip("Minimum cell distance required from another obstacle of the same category/type")]
    [SerializeField] private int typeRepulsionRadius = 4;

    [Tooltip("Relative probability weight when rolling from candidate pool")]
    [Range(0.01f, 10f)]
    [SerializeField] private float spawnWeight = 1.0f;

    public string ObstacleName => obstacleName;
    public GameObject Prefab => prefab;
    public ObstacleCategory Category => category;
    public Vector2Int FootprintSize => footprintSize;
    public int ClearanceBuffer => clearanceBuffer;
    public int TypeRepulsionRadius => typeRepulsionRadius;
    public float SpawnWeight => spawnWeight;
}
