using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CorridorFirstRoomGenerator : MapGenerator
{   
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;

    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // finds dead ends
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        // creates dead ends
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);
        
        floorPositions.UnionWith(roomPositions);

        tilemapVisualizer.PlaceFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        // finds dead ends
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }

            // adds dead end position to deadEnds, which will be used to create rooms in that position
            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }


    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>(); // room posisions
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent); // calculate the count of rooms selected from potentialRoomPositions

        // global unique identifier: sorted potential room positions and extracted the list of potential room positions at random
        List<Vector2Int> roomToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        
        // loop each position using randomWalk algorithm
        foreach (var roomPosition in roomToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition); // generate room at positions selected at random
            roomPositions.UnionWith(roomFloor);
        }
        return roomPositions;
    }

    private void CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition; 
        potentialRoomPositions.Add(currentPosition);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGeneration.RandomWalkCorridor(currentPosition, corridorLength); // generates corridor 
            currentPosition = corridor[corridor.Count - 1]; // sets next current position to the last position of the corridor, keeps them connected
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }
    }
}

