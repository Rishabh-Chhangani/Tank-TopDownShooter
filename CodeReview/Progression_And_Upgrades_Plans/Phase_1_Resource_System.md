# Phase 1: Resource System (XP & Currency)

## Objective
Establish the foundational economy for the game, allowing the player to collect resources dropped by defeated enemies to spend on upgrades.

## Key Tasks
1. **Drop System:**
   * Modify the `Damageable` or `EnemyAI` scripts to instantiate XP/Currency objects upon death.
   * Add a small randomization to the drop position to scatter multiple drops visually.
2. **Collection Logic:**
   * Create a `Pickup` script with a trigger collider.
   * Implement a magnetic effect where pickups smoothly fly towards the player when they get close.
3. **Resource Tracking:**
   * Add an `Inventory` or `PlayerStats` component to track the current XP and level of the tank.
