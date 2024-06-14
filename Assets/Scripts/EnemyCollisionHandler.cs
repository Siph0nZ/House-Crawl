using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private RoomFirstRoomGenerator roomGenerator;
    private HashSet<Vector2Int> floorTiles;

    private bool isOnMap = false;

    public void Initialize(RoomFirstRoomGenerator generator, HashSet<Vector2Int> floor)
    {
        roomGenerator = generator;
        floorTiles = floor;
    }

    void OnTriggerEnter2D(Collider2D touch)
    {   
        if (isOnMap == false)
        {
            Respawn();
        }

        else if (touch.gameObject.CompareTag("Floors"))
        {
            isOnMap = true;
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
