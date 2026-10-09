# Phase 4: Pickups & Reward Spawning

## Objective
Introduce procedural item drops (health, ammo, temporary buffs) into the generated arena, connecting the map generation directly to player progression and combat risk-reward systems.

## Key Tasks
1. **Pickup Spawner Creation**
   * Create a system that runs after enemy spawn points are calculated.
   * Pick 2-4 random valid floor tiles that are safely away from walls and obstacles to spawn pickups.

2. **Risk-Reward Logic**
   * Categorize the map layout. High-risk areas (e.g., the center of an "Outer Cover" layout, or deep inside a "Narrow Lane") should have a higher chance of spawning rare pickups (like full health or damage buffs).
   * Safe areas (near the player spawn) should spawn lower-tier items.

3. **Dynamic Drop Rates**
   * Hook this system into the enemy defeat events. If an enemy dies, the pickup spawner can dynamically place an item at the enemy's death location, validating that it didn't clip into a wall during the explosion.

4. **Integration with Future Systems**
   * Ensure the architecture allows easy additions of new pickup types, laying the groundwork for a larger progression framework down the line.
