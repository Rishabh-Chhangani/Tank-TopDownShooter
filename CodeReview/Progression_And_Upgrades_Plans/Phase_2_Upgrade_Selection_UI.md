# Phase 2: Upgrade Selection UI (Brotato Style)

## Objective
Create the interface that pauses the game between waves and presents the player with randomized choices to improve their tank.

## Key Tasks
1. **The Shop/Upgrade Screen:**
   * Design a UI Canvas that activates at the end of a wave.
   * Create 3-4 "Upgrade Card" UI prefabs that display the icon, name, description, and stat change of an upgrade.
2. **Randomized Pool:**
   * Create a ScriptableObject library of all possible upgrades (e.g., +10% Damage, +5 Max Health, +15% Move Speed).
   * Implement a weighted random selection algorithm to pick 3 distinct upgrades to offer the player.
3. **Selection Logic:**
   * Hook up UI buttons so selecting an upgrade applies it to the player, deducts currency (if applicable), and advances to the next wave.
