using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomMapGenerator: AbstractMapGenerator
{
    [SerializeField] protected SimpleRandomWalkSO simpleRandomWalkSO;
    
    protected override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloortile(floorPositions);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {

        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < simpleRandomWalkSO.iteration; i++)
        {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, simpleRandomWalkSO.walkLength);
            floorPositions.UnionWith(path);
            if (simpleRandomWalkSO.startRandomlyEachIteration)
                currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count));
        }
        return floorPositions;
    }
}
