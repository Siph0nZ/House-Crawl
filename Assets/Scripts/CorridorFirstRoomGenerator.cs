using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstRoomGenerator : MapGenerator
{   
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    [SerializeField]
    public RandomWalkSO roomGenerationParameters; // creates rooms after corridors 

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        CreateCorridors(floorPositions);

        tilemapVisualizer.PlaceFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions)
    {
        var currentPosition = startPosition; 

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGeneration.RandomWalkCorridor(currentPosition, corridorLength); // generates corridor 
            currentPosition = corridor[corridor.Count - 1]; // sets next current position to the last position of the corridor, keeps them connected
            floorPositions.UnionWith(corridor);
        }
    }
}

