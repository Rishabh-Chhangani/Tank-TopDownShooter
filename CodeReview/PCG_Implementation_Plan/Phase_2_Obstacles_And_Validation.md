# Phase 2: Obstacle Placement & Validation

## Objective
Populate the blocked areas generated in Phase 1 with actual game obstacles (rocks, walls, crates) while ensuring the map remains fully playable and tank-navigable.

## Key Tasks
1. **Obstacle Spawning System**
   * Create an `ObstacleSpawner` that reads the non-floor tiles from Phase 1.
   * Spawn corresponding obstacle prefabs (rocks, crates, walls) based on a weighted chance to add visual variety.
   * Ensure obstacle density remains balanced so that players and enemies always have room to aim and maneuver.

2. **Tank Clearance Validation**
   * Since tanks require a wider turning radius and space, implement a check (e.g., expanding the obstacle footprint by 1 tile) to guarantee that choke points are physically wide enough for the player and enemy tanks to pass through.

3. **Trapped Area Prevention (Flood Fill)**
   * Implement a Flood Fill (or A*) algorithm starting from the Player Spawn Zone.
   * Check if every walkable floor tile is reachable from the spawn.
   * If any "isolated islands" of floor exist, either fill them with obstacles or carve a path through the blocking walls to connect them to the main arena.
