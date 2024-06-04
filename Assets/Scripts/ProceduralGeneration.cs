using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGeneration
{
    public static HashSet<Vector2Int> RandomWalk(Vector2Int startPosition, int walkLength)
    {
        HashSet<Vector2Int> path = new HashSet<Vector2Int>(); 
        
        path.Add(startPosition);
        var previousPosition = startPosition;

        for (int i = 0; i < walkLength; i++)
        {
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection(); // gives random direction
            path.Add(newPosition); // adds new position to path
            previousPosition = newPosition; // sets new position as previous position
        }
        return path;
    }

    // selects single direction and walk through the corridors length
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var direction = Direction2D.GetRandomCardinalDirection(); // selects a direction
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        for (int i = 0; i < corridorLength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition); // first start position will not be added
        }
        return corridor;
    }
}

// generates random direction
public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        // x and y values
        new Vector2Int(0, 1), // goes up 
        new Vector2Int(1, 0), // goes right
        new Vector2Int(0, -1), // goes down
        new Vector2Int(-1, 0) // goes left 
    };

    // generates random direction
    public static Vector2Int GetRandomCardinalDirection()
    {
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}
