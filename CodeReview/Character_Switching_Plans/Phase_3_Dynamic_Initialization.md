# Phase 3: Dynamic Initialization

## Objective
Ensure the gameplay scene reads the selected character and properly builds the player object at runtime.

## Key Tasks
1. **Player Spawner:**
   * Remove the hardcoded Player prefab from the gameplay scene.
   * Add a `PlayerSpawner` object at the center of the map.
2. **Runtime Assembly:**
   * On scene start, the `PlayerSpawner` reads the selected character from the `SessionManager`.
   * It instantiates the base Player Prefab, swaps out the tank body/turret sprites based on the `CharacterData`, and initializes the stats components.
3. **Camera & UI Hookups:**
   * Dynamically assign the newly instantiated player to the `Cinemachine Virtual Camera` (or main camera follow script) and hook up the Health UI listeners.
