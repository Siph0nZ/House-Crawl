using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    [SerializeField]
    private GameObject spritePrefab; // Reference to the sprite prefab
    [SerializeField]
    private GameObject currentSprite;


    // PCG Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    } 

    private void CreateRooms()
    {
        var roomsList = ProceduralGeneration.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(roomWidth, roomHeight, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRoomsRandomly(roomsList);
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }
        List<Vector2Int> roomCenters = new List<Vector2Int>(); // stores center position of rooms
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center)); 
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors);

        tilemapVisualizer.PlaceFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);

        PlaceRandomSprite(floor); // places sprite
    }

    private void PlaceRandomSprite(HashSet<Vector2Int> floor)
    {

        if (currentSprite != null)
        {
            DestroyImmediate(currentSprite);
        }
        
        List<Vector2Int> floorTiles = new List<Vector2Int>(floor); // convert the HashSet to a List for easy random access
        Vector2Int randomTile = floorTiles[Random.Range(0, floorTiles.Count)];

        Vector3 worldPosition = new Vector3(randomTile.x, randomTile.y, 0); // convert tile position to world position

        // instantiate the sprite at the selected position and store the reference
        currentSprite = Instantiate(spritePrefab, worldPosition, Quaternion.identity);
    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i]; // select rooms
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y)); // calculate center points
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);

            foreach (var position in roomFloor)
            {
                if (position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset) && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset)) // apply offset
                {
                    floor.Add(position); // adds position to floor
                }
            }
        }
        return floor; 
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];

        // removes from room centers
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest); // 

            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor); // Add new corridor to corridors hashset
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>(); // define corridor
        var position = currentRoomCenter; // start position of corridor
        corridor.Add(position);

        // travel in up or down, or left or right direction until the destination is reached 
        while(position.y != destination.y)
        {
            if (destination.y > position.y) // up
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y) // down
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }

        while (position.x != destination.x)
        {
            if (destination.x > position.x) // right
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x) // left
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }

        // Extend the corridor by 1 tile in each direction
        List<Vector2Int> extendedPositions = new List<Vector2Int>();
        foreach (var pos in corridor)
        {
            // Add neighboring positions to extend the corridor
            extendedPositions.Add(pos + Vector2Int.up);
            extendedPositions.Add(pos + Vector2Int.down);
            extendedPositions.Add(pos + Vector2Int.left);
            extendedPositions.Add(pos + Vector2Int.right);
        }

    // Add the extended positions to the corridor
    corridor.UnionWith(extendedPositions);

        return corridor; // output of which direction to travel in to get to the destination (room center point)
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        // loop through each room center and find distance
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position, currentRoomCenter); // check distance between current room center and next room center
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        // loops through each room
        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

}

