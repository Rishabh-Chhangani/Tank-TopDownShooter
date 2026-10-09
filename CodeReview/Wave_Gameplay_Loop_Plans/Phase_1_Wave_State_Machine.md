# Phase 1: Wave State Machine

## Objective
Structure the gameplay loop into distinct, manageable states (Pre-wave, Active Wave, Intermission, Run Over).

## Key Tasks
1. **GameManager Refactor:**
   * Create a State Machine in `GameManager` (or `WaveManager`) with states: `MainMenu`, `WaveStarting`, `WaveActive`, `ShopPhase`, `GameOver`.
2. **State Transitions:**
   * **WaveStarting:** Show a UI banner ("Wave 1") and countdown. Disable player shooting/movement.
   * **WaveActive:** Enable combat, start the `EnemySpawner`, track active enemies.
   * **ShopPhase:** Triggered when `WaveManager` reports 0 enemies remaining. Pauses combat, opens Upgrade UI.
