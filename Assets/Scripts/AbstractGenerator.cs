using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null;

    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    // this generates each room
    public void GenerateRoom()
    {
        tilemapVisualizer.ClearFloor();
        RunProceduralGeneration();
    }

    protected abstract void RunProceduralGeneration();
}
