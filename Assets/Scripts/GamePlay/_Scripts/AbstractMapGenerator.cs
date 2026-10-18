using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapGenerator : MonoBehaviour
{
    [SerializeField] protected TileMapVisualizer tileMapVisualizer = null;
    [SerializeField] protected Vector2Int startPosition = Vector2Int.zero;
    public void GenerateMap()
    {
        if (tileMapVisualizer == null)
            tileMapVisualizer = GetComponent<TileMapVisualizer>();

        if (tileMapVisualizer == null)
        {
            Debug.LogError("TileMapVisualizer reference is missing on " + gameObject.name, this);
            return;
        }

        tileMapVisualizer.Clear();
        RunProceduralGeneration();
    }
    protected abstract void RunProceduralGeneration();
}
