# Phase 1: Character Data Architecture

## Objective
Decouple the player's tank from a static prefab, allowing different "Characters" or "Tanks" to define base statistics.

## Key Tasks
1. **Character ScriptableObject:**
   * Create a `CharacterData` SO containing fields for base health, base movement speed, starting weapon, and passive abilities (e.g., +10% damage).
2. **Data Structure:**
   * Organize all available characters in a central database or manager (e.g., a `CharacterDatabaseSO`).
3. **Player Controller Hookup:**
   * Ensure the `PlayerController` and `TankMovement` can accept a `CharacterData` object on initialization and apply those stats instead of serialized defaults.
