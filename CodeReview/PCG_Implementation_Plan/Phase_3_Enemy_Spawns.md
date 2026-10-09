# Phase 3: Enemy Spawn Integration

## Objective
Adapt the existing `SpawnManager.cs` and `EnemySpawner.cs` to work dynamically with the procedurally generated battlefield rather than relying on manually placed spawn points.

## Key Tasks
1. **Dynamic Spawn Point Registration**
   * After the map is validated in Phase 2, iterate through all valid floor tiles.
   * Filter out tiles that are too close to the Player Spawn Zone to ensure fairness (maintaining a minimum spawn radius).
   * Register the remaining valid tiles as potential `SpawnPoint` objects in a list.

2. **Directional & Wave Spawning**
   * Update the `EnemySpawner` to select from this dynamic list.
   * Implement logic to scatter enemy spawns. If an enemy spawns in the top-right, the next enemy in the wave should ideally spawn in a different quadrant (e.g., bottom-left) to surround the player dynamically instead of clustering.

3. **Collision Safety**
   * Ensure the selected dynamic spawn points have a clearance check (using `Physics2D.OverlapCircle`) to absolutely guarantee they aren't spawning clipping into rocks, walls, or other enemies.
