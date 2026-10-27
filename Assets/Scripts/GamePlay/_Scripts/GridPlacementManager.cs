using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the reservation or occupancy state of a single grid cell.
/// </summary>
public enum CellState
{
    Empty,
    BorderPadding,
    SafeZone,
    Occupied,
    ClearanceBuffer
}

/// <summary>
/// Tracks metadata for an obstacle placed on the grid.
/// </summary>
public struct PlacedObstacleRecord
{
    public Vector2Int RootPosition;
    public ObstacleDefinitionSO Definition;

    public PlacedObstacleRecord(Vector2Int rootPosition, ObstacleDefinitionSO definition)
    {
        RootPosition = rootPosition;
        Definition = definition;
    }
}

/// <summary>
/// Manages the discrete 2D occupancy grid, enforces navigation clearance buffers,
/// masks safe zones, and evaluates anti-clustering (type-repulsion) rules.
/// </summary>
public class GridPlacementManager
{
    private BoundsInt _bounds;
    private CellState[,] _grid;
    private readonly List<PlacedObstacleRecord> _placedObstacles = new List<PlacedObstacleRecord>();

    public BoundsInt Bounds => _bounds;
    public int Width => _bounds.size.x;
    public int Height => _bounds.size.y;
    public IReadOnlyList<PlacedObstacleRecord> PlacedObstacles => _placedObstacles;

    /// <summary>
    /// Initializes the grid with the specified arena bounds and sets up reserved masks.
    /// </summary>
    public void Initialize(
        BoundsInt arenaBounds,
        int borderPadding,
        Vector2Int playerSpawnGridPos,
        int playerSafeRadius,
        IEnumerable<Vector2Int> enemySpawnGridPositions,
        int enemySafeRadius)
    {
        _bounds = arenaBounds;
        int width = Mathf.Max(1, _bounds.size.x);
        int height = Mathf.Max(1, _bounds.size.y);

        _grid = new CellState[width, height];
        _placedObstacles.Clear();

        // 1. Mask border padding
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x < borderPadding || x >= width - borderPadding ||
                    y < borderPadding || y >= height - borderPadding)
                {
                    _grid[x, y] = CellState.BorderPadding;
                }
                else
                {
                    _grid[x, y] = CellState.Empty;
                }
            }
        }

        // 2. Mask player safe zone (circle around player spawn)
        Vector2Int playerLocal = WorldToGridCoords(playerSpawnGridPos);
        MaskCircularZone(playerLocal, playerSafeRadius, CellState.SafeZone);

        // 3. Mask enemy spawn safe zones
        if (enemySpawnGridPositions != null)
        {
            foreach (var enemySpawn in enemySpawnGridPositions)
            {
                Vector2Int enemyLocal = WorldToGridCoords(enemySpawn);
                MaskCircularZone(enemyLocal, enemySafeRadius, CellState.SafeZone);
            }
        }
    }

    /// <summary>
    /// Masks a circular area around a local grid coordinate with a specified state.
    /// </summary>
    private void MaskCircularZone(Vector2Int centerLocal, int radius, CellState state)
    {
        int rSqr = radius * radius;
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                if (dx * dx + dy * dy <= rSqr)
                {
                    int x = centerLocal.x + dx;
                    int y = centerLocal.y + dy;
                    if (IsInsideLocalBounds(x, y))
                    {
                        _grid[x, y] = state;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Converts a global grid position (matching Tilemap coordinates) to local grid index (0 to width-1).
    /// </summary>
    public Vector2Int WorldToGridCoords(Vector2Int globalGridPos)
    {
        return new Vector2Int(globalGridPos.x - _bounds.xMin, globalGridPos.y - _bounds.yMin);
    }

    /// <summary>
    /// Converts a local grid index (0 to width-1) back to global Tilemap grid coordinates.
    /// </summary>
    public Vector2Int GridToWorldCoords(Vector2Int localGridPos)
    {
        return new Vector2Int(localGridPos.x + _bounds.xMin, localGridPos.y + _bounds.yMin);
    }

    public bool IsInsideLocalBounds(int localX, int localY)
    {
        return localX >= 0 && localX < Width && localY >= 0 && localY < Height;
    }

    public CellState GetCellState(Vector2Int globalGridPos)
    {
        Vector2Int local = WorldToGridCoords(globalGridPos);
        if (!IsInsideLocalBounds(local.x, local.y))
            return CellState.BorderPadding;

        return _grid[local.x, local.y];
    }

    /// <summary>
    /// Checks if an obstacle footprint and its required clearance buffer can be placed at the target cell.
    /// </summary>
    public bool CanPlaceObstacle(Vector2Int globalGridPos, ObstacleDefinitionSO definition)
    {
        if (definition == null)
            return false;

        Vector2Int local = WorldToGridCoords(globalGridPos);

        // 1. Check physical footprint is completely empty
        Vector2Int foot = definition.FootprintSize;
        for (int fx = 0; fx < foot.x; fx++)
        {
            for (int fy = 0; fy < foot.y; fy++)
            {
                int x = local.x + fx;
                int y = local.y + fy;

                if (!IsInsideLocalBounds(x, y) || _grid[x, y] != CellState.Empty)
                    return false;
            }
        }

        // 2. Check clearance buffer around footprint (no adjacent occupied cells)
        int buf = definition.ClearanceBuffer;
        for (int bx = -buf; bx < foot.x + buf; bx++)
        {
            for (int by = -buf; by < foot.y + buf; by++)
            {
                // Skip the footprint cells themselves
                if (bx >= 0 && bx < foot.x && by >= 0 && by < foot.y)
                    continue;

                int x = local.x + bx;
                int y = local.y + by;

                if (IsInsideLocalBounds(x, y))
                {
                    // Buffer area cannot collide with an existing obstacle
                    if (_grid[x, y] == CellState.Occupied)
                        return false;
                }
            }
        }

        // 3. Anti-Clustering Check: Type-repulsion distance
        if (!PassesTypeRepulsion(globalGridPos, definition))
            return false;

        return true;
    }

    /// <summary>
    /// Verifies that no obstacle of the same category or definition exists within definition.TypeRepulsionRadius.
    /// </summary>
    public bool PassesTypeRepulsion(Vector2Int globalGridPos, ObstacleDefinitionSO definition)
    {
        float repulsionSqr = definition.TypeRepulsionRadius * definition.TypeRepulsionRadius;

        foreach (var placed in _placedObstacles)
        {
            // Check same category or exact definition
            if (placed.Definition == definition || placed.Definition.Category == definition.Category)
            {
                float distSqr = (placed.RootPosition - globalGridPos).sqrMagnitude;
                if (distSqr < repulsionSqr)
                {
                    return false; // Repelled: too close to an identical obstacle type
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Registers an obstacle on the grid, updating footprint cells to Occupied and surrounding cells to ClearanceBuffer.
    /// </summary>
    public void PlaceObstacle(Vector2Int globalGridPos, ObstacleDefinitionSO definition)
    {
        if (definition == null)
            return;

        Vector2Int local = WorldToGridCoords(globalGridPos);
        Vector2Int foot = definition.FootprintSize;
        int buf = definition.ClearanceBuffer;

        // Mark footprint
        for (int fx = 0; fx < foot.x; fx++)
        {
            for (int fy = 0; fy < foot.y; fy++)
            {
                int x = local.x + fx;
                int y = local.y + fy;
                if (IsInsideLocalBounds(x, y))
                {
                    _grid[x, y] = CellState.Occupied;
                }
            }
        }

        // Mark clearance buffer ring
        for (int bx = -buf; bx < foot.x + buf; bx++)
        {
            for (int by = -buf; by < foot.y + buf; by++)
            {
                int x = local.x + bx;
                int y = local.y + by;
                if (IsInsideLocalBounds(x, y) && _grid[x, y] == CellState.Empty)
                {
                    _grid[x, y] = CellState.ClearanceBuffer;
                }
            }
        }

        _placedObstacles.Add(new PlacedObstacleRecord(globalGridPos, definition));
    }

    /// <summary>
    /// Checks whether an entire mini-formation can be placed at the target coordinate.
    /// </summary>
    public bool CanPlaceFormation(Vector2Int globalRootPos, FormationDefinitionSO formation)
    {
        if (formation == null || formation.Elements.Count == 0)
            return false;

        foreach (var element in formation.Elements)
        {
            if (element.obstacle == null)
                continue;

            Vector2Int elementGlobalPos = globalRootPos + element.relativeOffset;
            if (!CanPlaceObstacle(elementGlobalPos, element.obstacle))
                return false;
        }

        return true;
    }

    /// <summary>
    /// Places all obstacles comprising a mini-formation at their relative offsets.
    /// </summary>
    public void PlaceFormation(Vector2Int globalRootPos, FormationDefinitionSO formation)
    {
        if (formation == null)
            return;

        foreach (var element in formation.Elements)
        {
            if (element.obstacle == null)
                continue;

            Vector2Int elementGlobalPos = globalRootPos + element.relativeOffset;
            PlaceObstacle(elementGlobalPos, element.obstacle);
        }
    }

    /// <summary>
    /// Retrieves all global grid cells currently in the Empty state.
    /// </summary>
    public List<Vector2Int> GetEmptyCandidateCells()
    {
        List<Vector2Int> candidates = new List<Vector2Int>();
        int width = Width;
        int height = Height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_grid[x, y] == CellState.Empty)
                {
                    candidates.Add(GridToWorldCoords(new Vector2Int(x, y)));
                }
            }
        }

        return candidates;
    }
}
