# Modular Arena Obstacle Generation System

## 1. System Overview

This system provides procedural obstacle and cover generation tailored for a fixed-arena, top-down tank shooter (inspired by *Brotato*). 

Instead of generating the floor layout from scratch, the system operates on the existing **Tilemap (`Ground`)** and boundaries (`Bounds.prefab`), procedurally populating the combat space with balanced, spaced, and varied obstacles each run.

### Core Design Pillars
1. **Guaranteed Tank Clearance**: Grid-cell placement with enforced clearance buffers ensures corridors are always at least 2.5–3.0 units wide so tanks (~1.17 units wide) never get pinned or snagged.
2. **Anti-Clustering Variety**: Local type-repulsion algorithms prevent duplicate obstacle types (e.g., three crates in a row) from spawning in the same immediate area.
3. **Mini-Formations & Standalones**: Procedural blending of standalone single props (isolated crates, barrels, trees) and coherent mini-formations (sandbag barricades, crate/barrel depots).
4. **Run Profiles**: Pre-configurable generation profiles (e.g., **Open Field** vs. **Tactical Battlefield**) that can be selected per run or scaled per wave.
5. **Interactive & Low-Cover Obstacles**: Built-in support for explosive red barrels, toxic/slow green barrels, and shoot-through sandbags.
6. **Zero-Code Extensibility**: Adding a new obstacle, barrel variant, or formation is 100% data-driven via ScriptableObjects.

---

## 2. Architecture & Data Structures

```
                 ┌──────────────────────────────────────┐
                 │        ArenaObstacleGenerator        │
                 │   (Extends AbstractMapGenerator)     │
                 └──────────────────┬───────────────────┘
                                    │
           ┌────────────────────────┼────────────────────────┐
           ▼                        ▼                        ▼
┌──────────────────────┐ ┌──────────────────────┐ ┌──────────────────────┐
│  ObstacleProfileSO   │ │ GridPlacementManager │ │ ObstacleCatalogSO    │
│  - Profile Type      │ │ - Cell Occupancy     │ │ - ObstacleDefinition │
│  - Target Density    │ │ - Clearance Buffer   │ │ - Formations         │
│  - Formation Ratio   │ │ - Safe Zones Mask    │ │ - Weighting & Rules  │
└──────────────────────┘ └──────────────────────┘ └──────────────────────┘
                                    │
                   ┌────────────────┴────────────────┐
                   ▼                                 ▼
        ┌──────────────────────┐          ┌──────────────────────┐
        │ Standalone Spawner   │          │  Formation Spawner   │
        └──────────────────────┘          └──────────────────────┘
```

### 2.1 ScriptableObjects

#### `ObstacleDefinitionSO`
Defines an individual obstacle type and its spatial/behavioral rules:
* `string obstacleName`: Unique identifier (e.g. "Crate_Wood", "Red_Barrel", "Sandbag").
* `GameObject prefab`: Prefab to instantiate.
* `ObstacleCategory category`: Enum (`SolidCover`, `LowCover`, `ExplosiveHazard`, `ToxicHazard`).
* `Vector2Int footprintSize`: Cells occupied on the grid (e.g. $1 \times 1$, $1 \times 2$).
* `int clearanceBuffer`: Minimum required empty cells around this obstacle (default: 2 cells).
* `int typeRepulsionRadius`: Minimum cell distance allowed before another obstacle of the *same category/type* can be placed (prevents clumping).
* `float spawnWeight`: Relative spawn frequency in the generation pool.

#### `FormationDefinitionSO`
Defines multi-tile mini-formations:
* `string formationName`: (e.g. "Sandbag_L_Wall", "Crate_Barrel_Depot").
* `List<FormationCell> elements`:
  * `Vector2Int relativeOffset`: Coordinate relative to formation root $(0,0)$.
  * `ObstacleDefinitionSO obstacle`: Specific obstacle to place at that offset.
* `Vector2Int totalBoundingSize`: Enclosed area including required perimeter clearance.
* `float spawnWeight`: Chance to choose this formation.

#### `ObstacleProfileSO`
Configures the overall feel of a run:
* `string profileName`: (e.g. "Open Field", "Tactical Battlefield", "Heavy Ambush").
* `Vector2Int targetObstacleCount`: Minimum and maximum total obstacle nodes (e.g., [8, 14] for Open Field vs. [20, 26] for Tactical).
* `float formationChance`: Probability (0.0 to 1.0) that a placement node spawns a mini-formation instead of a single standalone object.
* `List<ObstacleDefinitionSO> allowedObstacles`: Whitelist of obstacles valid for this profile.
* `List<FormationDefinitionSO> allowedFormations`: Whitelist of formations valid for this profile.

---

## 3. Grid & Placement Algorithm

### Step 1: Arena Grid Initialization
1. Query the `Ground` Tilemap bounds (`cellBounds`) or use the configured `arenaSize` (e.g., $30 \times 15$).
2. Initialize a 2D occupancy grid: `CellState[width, height]`.
3. Mark unplayable borders: Apply an outer padding (2 cells inward from arena walls) as `Reserved_Border` so tanks can freely circle the perimeter.
4. Mark player spawn safe zone: Circle/box of 5 cells around player start point marked as `Reserved_SafeZone`.
5. Mark enemy spawner clearances: 3-cell radius around each `SpawnPoint` marked as `Reserved_SafeZone`.

### Step 2: Candidate Node Selection
1. Collect all candidate cells marked `Empty`.
2. Shuffle candidate list (Fisher-Yates) to ensure deterministic randomness based on seed.
3. For each candidate coordinate $(X, Y)$:
   * Check if cell $(X, Y)$ is free and has the required `clearanceBuffer` from any existing obstacle.
   * If clearance check passes, decide whether to place a **Mini-Formation** (based on `profile.formationChance`) or a **Standalone Obstacle**.

### Step 3: Anti-Clustering Selection
1. Evaluate all obstacles in the active pool.
2. Filter out any obstacle type whose `typeRepulsionRadius` overlaps an existing obstacle of the same type within that cell's neighborhood.
3. From the remaining valid candidates, select via weighted random roll.
4. Mark the occupied grid cells as `Occupied(ObstacleType)` and the surrounding buffer ring as `Reserved_Buffer`.

---

## 4. Interactive & Low-Cover Obstacles

### 4.1 Red Barrel (Explosive AOE)
* **Component**: `ExplosiveBarrel : MonoBehaviour, IDamageable`
* **Trigger Conditions**:
  * **Bullet Hit**: Accumulates damage; on $HP \le 0$, detonates.
  * **Collision / Ramming**: When hit by a tank (`OnCollisionEnter2D`), if tank velocity exceeds threshold, detonates immediately, dealing damage to both the tank and the surrounding radius.
* **Explosion**: Spawns `Explosion.prefab`, plays sound feedback, queries all 2D colliders in radius, and calls `TakeDamage()` on player and enemies alike.

### 4.2 Green Barrel (Toxic Sludge / Speed Penalty)
* **Component**: `ToxicBarrel : MonoBehaviour, IDamageable`
* **Trigger Conditions**:
  * On break / collision, shatters and deals contact damage.
  * Spawns a temporary `HazardPuddle` (`oilSpill` sprite) with a 2D trigger collider lasting $N$ seconds.
* **Hazard Puddle Effect**:
  * When a tank enters the trigger, applies `SlowModifier(0.5f)` to `TankMovement.cs` and deals minor periodic damage.
  * Cleans up automatically after duration expires.

### 4.3 Sandbags (Low Cover - Shoot-Through)
* **Status**: **Already Implemented & Pre-Configured** (Spawn-Only).
* **Prefab & Layer Configuration**:
  * Existing prefab: `Assets/Prefab/Snad Bag.prefab`.
  * Layer assignment: Assigned to layer 9 (`ObstacleMovement`).
  * In **Physics2D Collision Matrix**:
    * `ObstacleMovement` vs `Agent` / `Player` / `Enemy`: **Enabled** (Tanks cannot pass through sandbags).
    * `ObstacleMovement` vs `Bullet`: **Disabled** (Bullets pass directly through sandbags).
  * Result: Provides tactical blocking against tank movement while permitting crossfire.
* **Procedural Integration**:
  * No new components or collision matrix adjustments are required.
  * Simply assign `Snad Bag.prefab` to an `ObstacleDefinitionSO` (category: `LowCover`) and reference it in standalone obstacle pools or `FormationDefinitionSO` (e.g., barricade lines/bunkers) to spawn it procedurally.

---

## 5. Phase-by-Phase Implementation Plan

| Phase | Milestone | Deliverables |
|---|---|---|
| **Phase 1** | **Data Architecture** | Create `ObstacleDefinitionSO`, `FormationDefinitionSO`, `ObstacleProfileSO`, and create initial asset definitions in `Assets/Data/PCG/`. |
| **Phase 2** | **Grid & Clearance Logic** | Create `GridPlacementManager.cs` with cell occupancy, safe zone masking, buffer checks, and local type-repulsion. |
| **Phase 3** | **Generator Integration** | Create `ArenaObstacleGenerator.cs` (extending `AbstractMapGenerator`), hooking into `RandomMapGeneratorEditor` for 1-click generation and clearing in Editor. |
| **Phase 4** | **Interactive Mechanics** | Implement `ExplosiveBarrel.cs`, `ToxicBarrel.cs`, and `HazardPuddle.cs`. (Sandbag functionality and layering are already fully implemented via `ObstacleMovement` on `Snad Bag.prefab`; only needs `ObstacleDefinitionSO` asset to spawn). |
| **Phase 5** | **Polish & Playtest** | Verify tank navigation clearance, tune Open Field vs. Tactical Battlefield profiles, and verify enemy AI navigation. |

---

## 6. How to Add New Obstacles (Extensibility)

Because the system is data-driven, adding any future obstacle requires zero changes to the core generator:
1. **Create the Prefab**: Drag any sprite into a prefab, attach a 2D collider (and optional `Damageable` / barrel script).
2. **Create the Definition**: Right-click in Project view $\rightarrow$ `Create > PCG > Obstacle Definition`. Assign the prefab, footprint size, and repulsion radius.
3. **Add to Profile**: Add the definition into any `ObstacleProfileSO` (e.g., Open Field or Tactical). The generator will automatically include it in the next run.
