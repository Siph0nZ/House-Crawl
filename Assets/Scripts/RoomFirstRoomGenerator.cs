using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

// Add generation of AI enemy's that follow you around the map
// Idea - Weeping Angels + Time out system

public class RoomFirstRoomGenerator : MapGenerator
{   
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int roomWidth = 20, roomHeight = 20;
    [SerializeField]
    private int numItems = 10;
    [SerializeField]
    private int numEnemies = 5;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false; // check if random walk is enabled

    // player sprite
    [SerializeField]
    private GameObject spritePrefab; // Reference to the sprite prefab
    [SerializeField]
    private GameObject currentSprite; // Reference to the current spawned in sprite prefab

    // enemy
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject currentEnemy;
    private List<GameObject> spawnedEnemies = new List<GameObject>(); // list to store references of spawned collectibles

    // collectibles
    [SerializeField]
    private GameObject collectiblePrefab;
    [SerializeField]
    private GameObject currentCollectible;
    private List<GameObject> spawnedCollectibles = new List<GameObject>(); // list to store references of spawned collectibles

    // PCG Data
    private Dictionary<Vector2Int, HashSet<Vector2Int>> roomsDictionary = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

    // generates room when pressed play on menu scene
    private void Start()
    {
        tilemapVisualizer.ClearFloor();
        RunProceduralGeneration();
    }

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
        PlaceRandomEnemy(floor); // places enemy
        PlaceRandomCollectible(floor); // places collectibles
    }

    private void PlaceRandomCollectible(HashSet<Vector2Int> floor)
    {   
        List<Vector2Int> floorTiles = new List<Vector2Int>(floor); // convert the HashSet to a List for easy random access

        foreach (var collectible in spawnedCollectibles) // counts number of collectibles
        {
            DestroyImmediate(collectible);
        }
        spawnedCollectibles.Clear(); // clears spawned collectibles before creating new ones

        // spawning
        for (int i = 0; i < numItems; i++)
        {   
            Vector2Int randomTile = floorTiles[Random.Range(0, floorTiles.Count)];
            Vector3 worldPosition = new Vector3(randomTile.x, randomTile.y, 0); // convert tile position to world position
            currentCollectible = Instantiate(collectiblePrefab, worldPosition, Quaternion.identity);

            // adds collision handler script to initialize it
            SpriteCollisionHandler collisionHandler = currentCollectible.AddComponent<SpriteCollisionHandler>();
            collisionHandler.Initialize(this, floor); // initializes the collision handler
            
            spawnedCollectibles.Add(currentCollectible); // adds collectibles to currentCollectible
        }
    }

    private void PlaceRandomEnemy(HashSet<Vector2Int> floor)
    {
        foreach (var enemy in spawnedEnemies)
        {
            DestroyImmediate(enemy);
        }
        spawnedEnemies.Clear(); // clears spawned collectibles before creating new ones

        // spawning
        for (int i = 0; i < numEnemies; i++)
        {
            List<Vector2Int> floorTiles = new List<Vector2Int>(floor); // convert the HashSet to a List for easy random access
            Vector2Int randomTile = floorTiles[Random.Range(0, floorTiles.Count)]; // select a random tile from the floor
            Vector3 worldPosition = new Vector3(randomTile.x, randomTile.y, 0); // convert tile position to world position
            currentEnemy = Instantiate(enemyPrefab, worldPosition, Quaternion.identity); // instantiate the enemy at the selected position and store the reference
            spawnedEnemies.Add(currentEnemy); // adds enemies to currentEnemies
        }
    }

    private void PlaceRandomSprite(HashSet<Vector2Int> floor)
    {
        if (currentSprite != null)
        {
            DestroyImmediate(currentSprite);
        }
 
        List<Vector2Int> floorTiles = new List<Vector2Int>(floor); // convert the HashSet to a List for easy random access
        Vector2Int randomTile = floorTiles[Random.Range(0, floorTiles.Count)]; // select a random tile from the floor
        Vector3 worldPosition = new Vector3(randomTile.x, randomTile.y, 0); // convert tile position to world position
        currentSprite = Instantiate(spritePrefab, worldPosition, Quaternion.identity); // instantiate the sprite at the selected position and store the reference
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

