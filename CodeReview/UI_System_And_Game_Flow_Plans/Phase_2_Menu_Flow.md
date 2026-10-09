# Phase 2: Menu Flow (Pause, Main, Game Over)

## Objective
Establish complete front-end to back-end navigation through standard game menus.

## Key Tasks
1. **Main Menu Scene:**
   * Create a dedicated `MainMenu` scene with Play, Settings, and Quit buttons.
   * Add a screen to select the starting tank (hooked up to Character Switching).
2. **Pause Menu:**
   * Hook into the new input system's Pause action (`OnPause`).
   * Create a UI Canvas that sets `Time.timeScale = 0f` and provides Resume/Quit to Menu options.
3. **Game Over Screen:**
   * Design a screen that fades in upon death or victory, displaying stats (Waves Survived, Enemies Killed) and a Return to Menu button.
