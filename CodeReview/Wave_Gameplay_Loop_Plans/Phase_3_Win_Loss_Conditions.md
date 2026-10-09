# Phase 3: Win/Loss Conditions

## Objective
Provide clear closure to the gameplay loop with success or failure states.

## Key Tasks
1. **Player Death (Loss):**
   * Hook into the Player's `Damageable.OnDeath` event.
   * Trigger time slowdown (`Time.timeScale = 0.2f`), show a Death Effect, and pop up the "Game Over" screen with a button to restart.
2. **Run Completion (Win):**
   * Define a final wave (e.g., Wave 20).
   * Upon clearing the final wave, trigger a "Victory" UI state instead of the Shop.
   * Save high scores (e.g., Waves Survived, Enemies Defeated) using the `SaveSystem`.
