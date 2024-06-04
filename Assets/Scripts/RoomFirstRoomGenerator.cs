using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFirstRoomGenerator : MapGenerator
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int roomWidth = 20, roomHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false; // check if random walk is enabled

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    } 

    private void CreateRooms()
    {
        var roomList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(roomWidth, roomHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomList);

        tilemapVisualizer.PlaceFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        // loops through each room
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

}

