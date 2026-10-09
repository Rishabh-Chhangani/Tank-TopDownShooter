# Phase 2: Selection Menu

## Objective
Build the interface where players can view and select different tanks before a run.

## Key Tasks
1. **Roster UI:**
   * Create a "Select Character" screen in the Main Menu scene.
   * Dynamically generate UI buttons for each `CharacterData` in the database.
2. **Preview Panel:**
   * Display the selected tank's sprite, name, and base stats (health, speed, damage modifier) in a panel next to the selection grid.
3. **Persistent Selection:**
   * Save the chosen `CharacterData` index using an intermediate manager (like `GameManager` or `SessionManager`) marked with `DontDestroyOnLoad` so it persists into the gameplay scene.
