# Phase 3: Stat Modification System

## Objective
Connect the chosen upgrades to the actual gameplay scripts so that the tank's performance scales dynamically.

## Key Tasks
1. **Stat Container:**
   * Refactor `TankMovementData` and `TurretData` usages. Instead of reading directly from the ScriptableObject, the player's tank should instantiate a runtime copy of these stats (`CurrentStats`).
2. **Modifier Logic:**
   * Implement an `ApplyUpgrade(UpgradeData)` function that permanently modifies the `CurrentStats` (e.g., multiplying max speed, reducing reload delay, increasing bullet damage).
3. **Visual Reflection (Optional Polish):**
   * Change bullet colors, sizes, or tank exhaust particles based on certain powerful upgrades to give the player visual feedback of their progression.
