# Procedural Map Generator Instructions

The project contains a modular procedural generation system under the `Assets/Scripts/GamePlay/_Scripts` and `Assets/Editor` folders.

## System Architecture

The Procedural Generator logic relies on 4 main components:
1. **`AbstractMapGenerator`**: The base abstract class acting as the skeleton for generating maps. It clears the tilemap and triggers procedural algorithms.
2. **`SimpleRandomMapGenerator`**: The implementation of the abstract class. It relies on a "Random Walk" algorithm provided by the helper class `ProceduralGenerationAlgorithms.cs` to generate the level paths. 
3. **`TileMapVisualizer`**: Responsible for actually drawing the generated floors. It acts as the bridge between raw map data (`HashSet<Vector2Int>`) and Unity's Tilemap system.
4. **`SimpleRandomWalkSO` (ScriptableObject Data)**: Stores the configuration for the map (iterations, walk length, starting randomly).

## How to Setup and Use it in the Game

1. **Scene Setup:**
   * Create an empty `GameObject` in your Scene and name it `MapGenerator`.
   * Add the `SimpleRandomMapGenerator` script to this GameObject.
   * Add the `TileMapVisualizer` script to the same GameObject.
   
2. **Tilemap Setup:**
   * Right-click the Unity Hierarchy: **2D Object > Tilemap > Rectangular**. This creates a Grid with a Tilemap.
   * Drag your new `Tilemap` into the **Floor Tile Map** field on the `TileMapVisualizer` component.
   * Assign your chosen floor `TileBase` object (e.g. your floor sprite tile asset) to the **Floor Tile** field.
   * Drag the `TileMapVisualizer` component into the `tileMapVisualizer` field of the `SimpleRandomMapGenerator`.

3. **Data Configuration (Scriptable Object):**
   * Go into the Project view.
   * Right-click -> **Create > PCG > SimpleRandomWalkData**.
   * Select the newly created data file and adjust its parameters in the Inspector (e.g., `iteration = 10`, `walkLength = 10`).
   * Drag this Scriptable Object into the **SimpleRandomWalkSO** field of the `SimpleRandomMapGenerator`.

4. **Generating the Map:**
   * Because of the custom editor script (`RandomMapGeneratorEditor.cs`), you can test the generator without hitting Play.
   * Select your `MapGenerator` GameObject.
   * In the Inspector, look at the `SimpleRandomMapGenerator` component.
   * Click the custom **"Generate Map"** button. The floor tiles will be painted onto your Tilemap instantly. 
   
*Note: Due to typo naming in the scripts, be aware that it's typed as `AbstractMapGenerator` and `SimpleRandomMapGenerator` missing the 'e' in Generator.*
