using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;

public class Path
{
    private List<GameObject> path = new List<GameObject>();
    private List<GameObject> topTiles = new List<GameObject>();
    private List<GameObject> bottomTIles = new List<GameObject>();

    private int radius;
    private int currentTilesIndex;

    private bool hasReachedX;
    private bool hasReachedZ;

    private GameObject startTile;
    private GameObject endTile;

    public List<GameObject> GetPath()
    {
        return path;
    }
    public Path(int radius)
    {
        this.radius = radius;
    }

    public void AssignTopAndBottomTiles(int z, GameObject tile)
    {
        if (z == 2) // Adjusted to start from the second row
            topTiles.Add(tile);
        if (z == radius - 2) // Adjusted to end at the second-to-last row
            bottomTIles.Add(tile);
    }

    private bool AssignAndCheckStartingAndEndingTile()
    { 
        int xIndex = Random.Range(0, topTiles.Count - 1);
        int zIndex = Random.Range(0, bottomTIles.Count - 1);

        startTile = topTiles[xIndex];
        endTile = bottomTIles[zIndex];

        return startTile != null && endTile != null;

    }

    public void GeneratePath()
    {
        if (AssignAndCheckStartingAndEndingTile())
        {
            // Define a midpoint
            Vector3 midpointPosition = new Vector3(
                (startTile.transform.position.x + endTile.transform.position.x) / 2,
                startTile.transform.position.y,
                (startTile.transform.position.z + endTile.transform.position.z) / 4
            );

            GameObject currentTile = startTile;

            // Move towards the midpoint
            var safetyBrakeToMidpoint = 0;
            while (Vector3.Distance(currentTile.transform.position, midpointPosition) > 0.1f)
            {
                safetyBrakeToMidpoint++;
                if (safetyBrakeToMidpoint > 100) break;

                if (currentTile.transform.position.x > midpointPosition.x)
                    MoveDown(ref currentTile);
                else if (currentTile.transform.position.x < midpointPosition.x)
                    MoveUp(ref currentTile);

                if (currentTile.transform.position.z > midpointPosition.z)
                    MoveRight(ref currentTile);
                else if (currentTile.transform.position.z < midpointPosition.z)
                    MoveLeft(ref currentTile);
            }

            // Move from the midpoint to the end tile
            var safetyBrakeToEnd = 0;
            while (!hasReachedX || !hasReachedZ)
            {
                safetyBrakeToEnd++;
                if (safetyBrakeToEnd > 100) break;

                if (!hasReachedX)
                {
                    if (currentTile.transform.position.x > endTile.transform.position.x)
                        MoveDown(ref currentTile);
                    else if (currentTile.transform.position.x < endTile.transform.position.x)
                        MoveUp(ref currentTile);
                    else
                        hasReachedX = true;
                }

                if (!hasReachedZ)
                {
                    if (currentTile.transform.position.z > endTile.transform.position.z)
                        MoveRight(ref currentTile);
                    else if (currentTile.transform.position.z < endTile.transform.position.z)
                        MoveLeft(ref currentTile);
                    else
                        hasReachedZ = true;
                }
            }

            path.Add(endTile);
        }

    }


    private void MoveDown(ref GameObject currentTile)
    {
        if (!path.Contains(currentTile)) // Check if the tile is already in the path
            path.Add(currentTile);

        currentTilesIndex = WorldGenerator.GeneratedTiles.IndexOf(currentTile);
        int n = currentTilesIndex - radius;
        currentTile = WorldGenerator.GeneratedTiles[n];
    }

    private void MoveUp(ref GameObject currentTile)
    {
        if (!path.Contains(currentTile)) // Check if the tile is already in the path
            path.Add(currentTile);

        currentTilesIndex = WorldGenerator.GeneratedTiles.IndexOf(currentTile);
        int n = currentTilesIndex + radius;
        currentTile = WorldGenerator.GeneratedTiles[n];
    }

    private void MoveLeft(ref GameObject currentTile)
    {
        if (!path.Contains(currentTile)) // Check if the tile is already in the path
            path.Add(currentTile);

        currentTilesIndex = WorldGenerator.GeneratedTiles.IndexOf(currentTile);
        currentTilesIndex++;
        currentTile = WorldGenerator.GeneratedTiles[currentTilesIndex];
    }

    private void MoveRight(ref GameObject currentTile)
    {
        if (!path.Contains(currentTile)) // Check if the tile is already in the path
            path.Add(currentTile);

        currentTilesIndex = WorldGenerator.GeneratedTiles.IndexOf(currentTile);
        currentTilesIndex--;
        currentTile = WorldGenerator.GeneratedTiles[currentTilesIndex];
    }



}

