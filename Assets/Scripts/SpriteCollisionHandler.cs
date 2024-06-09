using UnityEngine;
using System.Collections.Generic;

public class SpriteCollisionHandler : MonoBehaviour
{
    private RoomFirstRoomGenerator roomGenerator;
    private HashSet<Vector2Int> floorTiles;

    public void Initialize(RoomFirstRoomGenerator generator, HashSet<Vector2Int> floor)
    {
        roomGenerator = generator;
        floorTiles = floor;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {   
        if (collision.gameObject.CompareTag("Walls"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        // Convert the HashSet to a List for easy random access
        List<Vector2Int> floorTileList = new List<Vector2Int>(floorTiles);
        Vector2Int randomTile = floorTileList[Random.Range(0, floorTileList.Count)];
        Vector3 worldPosition = new Vector3(randomTile.x, randomTile.y, 0); // Convert tile position to world position

        transform.position = worldPosition;
    }
}