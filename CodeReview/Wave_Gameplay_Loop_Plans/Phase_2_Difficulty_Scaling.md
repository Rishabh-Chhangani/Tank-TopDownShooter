# Phase 2: Difficulty Scaling

## Objective
Make each successive wave harder to challenge the player's increasing power level.

## Key Tasks
1. **Wave Data ScriptableObjects:**
   * Create a `WaveData` SO that dictates how many enemies to spawn, what types of enemies, and the spawn rate for a specific wave.
2. **Dynamic Scaling Logic:**
   * Alternatively, use an algorithmic approach where `Wave Count` multiplies enemy stats (e.g., `EnemyHealth = BaseHealth * (1 + (Wave * 0.2f))`).
3. **Elite Enemies:**
   * Introduce a chance to spawn slightly larger, different colored tanks with higher stats in later waves.
