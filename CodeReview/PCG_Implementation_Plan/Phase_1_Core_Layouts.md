# Phase 1: Core Map Generation & Encounter Layouts

## Objective
Establish the foundational procedural generation system tailored specifically for this top-down tank shooter. Focus on creating different arena shapes and ensuring the player's spawn point is safe.

## Key Tasks
1. **Grid & Arena Setup**
   * Define the boundaries of the combat arena using a tile-based grid system (extending your existing `TileMapVisualizer`).
   * Set up a designated "Player Spawn Zone" (e.g., the center of the map) that is strictly protected from any obstacle generation.

2. **Implement Encounter Patterns**
   * Instead of purely random generation (which can create chaotic maps), implement a room/pattern selection system.
   * Create algorithms for the 4 primary encounter layouts:
     * **Open Arena:** Very sparse obstacles, focusing on dodging and long-range shooting.
     * **Narrow Lanes:** Clustered obstacles that create tight choke points.
     * **Central Island:** A massive obstacle structure in the middle, forcing circular combat.
     * **Outer Cover:** Cover scattered along the edges, leaving the center exposed.

3. **Tile Painting**
   * Connect the pattern algorithms to the `SimpleRandomMapGenerator` (or create a new `ArenaMapGenerator`).
   * Paint floor tiles for valid areas and designate blocked areas for Phase 2.
