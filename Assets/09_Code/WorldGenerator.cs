using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static List<GameObject> GeneratedTiles = new List<GameObject>();

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject pathTilePrefab; // Prefab for Path tiles
    [SerializeField] private GameObject playerPrefab;   // Prefab for the player#
    [SerializeField] private GameObject housePrefab;   // Prefab for the house
    [SerializeField] private GameObject boundingBoxPrefab;
    [SerializeField] private GameObject[] treePrefabs;  // Array of tree prefabs

    private int radius = 50;

    void Start()
    {
        Path pathGenerator = new Path(radius);

        for (int x = 0; x < radius; x++)
        {
            for (int z = 0; z < radius; z++)
            {
                GameObject tile = Instantiate(tilePrefab,
                    new Vector3(x * 10f, 0, z * 10f), Quaternion.identity);

                GeneratedTiles.Add(tile);
                pathGenerator.AssignTopAndBottomTiles(z, tile);
            }
        }

        pathGenerator.GeneratePath();
        foreach (var pObject in pathGenerator.GetPath())
        {
            ReplaceWithPathTile(pObject); // Replace the tile with a Path tile
        }

        // Instantiate the player on the start tile
        InstantiatePlayerOnStartTile(pathGenerator);

        // Add trees to tiles
        AddTreesToTiles(pathGenerator);

        AddHouseOnLastTile(pathGenerator);

        AddBoundingBox(pathGenerator);
    }

    private void ReplaceWithPathTile(GameObject originalTile)
    {
        // Replace the original tile with a Path tile
        Vector3 position = originalTile.transform.position;
        Quaternion rotation = originalTile.transform.rotation;

        Destroy(originalTile);
        Instantiate(pathTilePrefab, position, rotation);
    }

    private void InstantiatePlayerOnStartTile(Path pathGenerator)
    {
        GameObject startTile = pathGenerator.GetPath()[0]; // Get the start tile
        GameObject nextTile = pathGenerator.GetPath()[1];  // Get the next tile to determine direction

        // Calculate the direction to face
        Vector3 direction = (nextTile.transform.position - startTile.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 playerPosition = startTile.transform.position + new Vector3(0, 1, 0);

        // Instantiate the player at the start tile's position and face the direction of the path
        Instantiate(playerPrefab, playerPosition, rotation);
    }

    private void AddTreesToTiles(Path pathGenerator)
    {
        foreach (var tile in GeneratedTiles)
        {
            // Skip tiles that are part of the path
            if (pathGenerator.GetPath().Contains(tile))
                continue;

            // Randomly decide how many trees to place on this tile
            int treeCount = Random.Range(2, 3);

            for (int i = 0; i < treeCount; i++)
            {
                // Randomly select a tree prefab
                GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                Vector3 randomOffset = new Vector3(
                    Random.Range(-1f, 1f),
                    -0.4f,
                    Random.Range(-1f, 1f)
                );
                Vector3 treePosition = tile.transform.position + randomOffset;

                // Instantiate the tree
                GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);

                // Randomize the tree's size
                float randomScale = Random.Range(3f, 7f); // Adjust range as needed
                tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
    }

    private void AddHouseOnLastTile(Path pathGenerator)
    {
        GameObject lastTile = pathGenerator.GetPath()[pathGenerator.GetPath().Count - 1];
        Vector3 housePosition = lastTile.transform.position + new Vector3(0, 1, 0);
        Instantiate(housePrefab, housePosition, Quaternion.identity);

    }

    private void AddBoundingBox(Path pathGenerator)
    {
        HashSet<Vector3> boundingBoxPositions = new HashSet<Vector3>();

        foreach (var pathTile in pathGenerator.GetPath())
        {
            Vector3 pathTilePosition = pathTile.transform.position;

            // Check adjacent positions (up, down, left, right)
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(20f, 0, 0), boundingBoxPositions, pathGenerator); // Right
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(-20f, 0, 0), boundingBoxPositions, pathGenerator); // Left
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(0, 0, 20f), boundingBoxPositions, pathGenerator); // Up
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(0, 0, -20f), boundingBoxPositions, pathGenerator); // Down
        }
    }

    private void AddBoundingBoxTileAtPosition(Vector3 position, HashSet<Vector3> boundingBoxPositions, Path pathGenerator)
    {
        // Check if the position is already occupied by a bounding box
        if (boundingBoxPositions.Contains(position))
            return;

        // Check if the position corresponds to a path tile
        foreach (var pathTile in pathGenerator.GetPath())
        {
            if (Vector3.Distance(pathTile.transform.position, position) < 0.1f)
            {
                // Skip adding a bounding box above a path tile
                return;
            }
        }

        // Instantiate a bounding box tile at the position
        Instantiate(boundingBoxPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
        boundingBoxPositions.Add(position);
    }
}


