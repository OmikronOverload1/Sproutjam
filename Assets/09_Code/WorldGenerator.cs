using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static List<GameObject> GeneratedTiles = new List<GameObject>();
    private List<GameObject> treeInstances = new List<GameObject>(); // Track all instantiated trees

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject pathTilePrefab; // Prefab for Path tiles
    [SerializeField] private GameObject playerPrefab;   // Prefab for the player
    [SerializeField] private GameObject housePrefab;   // Prefab for the house
    [SerializeField] private GameObject boundingBoxPrefab; // Prefab for the bounding box
    [SerializeField] private GameObject[] treePrefabs;  // Array of tree prefabs
    [SerializeField] private GameObject playerInstance; // Track the instantiated player

    private int radius = 50;

    private void Start()
    {
        GenerateWorld();
    }


    public void RegenerateWorld()
    {
        Debug.Log("RegenerateWorld called");

        // Destroy the player instance
        if (playerInstance != null)
        {
            Debug.Log($"Destroying player instance: {playerInstance.name}");
            Destroy(playerInstance);
            playerInstance = null;
        }

        // Clear existing tiles
        foreach (var tile in GeneratedTiles)
        {
            Destroy(tile);
        }
        GeneratedTiles.Clear();

        // Clear existing trees
        treeInstances.RemoveAll(tree => tree == null); // Remove null references
        foreach (var tree in treeInstances)
        {
            Debug.Log($"Destroying tree: {tree.name}");
            Destroy(tree);
        }
        treeInstances.Clear();

        // Recreate the world
        GenerateWorld();
    }

    private void GenerateWorld()
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

        // Add a house on the last tile
        AddHouseOnLastTile(pathGenerator);

        // Add bounding box tiles around the path
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
        // Destroy the previous player instance if it exists
        if (playerInstance != null)
        {
            Debug.Log($"Destroying previous player instance: {playerInstance.name}");
            Destroy(playerInstance);
        }

        GameObject startTile = pathGenerator.GetPath()[0]; // Get the start tile
        GameObject nextTile = pathGenerator.GetPath()[1];  // Get the next tile to determine direction

        // Calculate the direction to face
        Vector3 direction = (nextTile.transform.position - startTile.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Vector3 playerPosition = startTile.transform.position + new Vector3(0, 1, 0);

        // Instantiate the player at the start tile's position and face the direction of the path
        playerInstance = Instantiate(playerPrefab, playerPosition, rotation);
        Debug.Log($"New player instance created: {playerInstance.name}");
    }

    private void AddTreesToTiles(Path pathGenerator)
    {
        foreach (var tile in GeneratedTiles)
        {
            // Skip tiles that are part of the path
            if (pathGenerator.GetPath().Contains(tile))
                continue;

            // Randomly decide how many trees to place on this tile
            int treeCount = Random.Range(3, 7);

            for (int i = 0; i < treeCount; i++)
            {
                // Randomly select a tree prefab
                GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                Vector3 randomOffset = new Vector3(
                    Random.Range(-4f, 4f), // Adjusted range for better placement
                    -0.4f,
                    Random.Range(-4f, 4f)
                );
                Vector3 treePosition = tile.transform.position + randomOffset;

                // Instantiate the tree
                GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);

                // Add the tree to the list of tree instances
                treeInstances.Add(tree);
                Debug.Log($"Tree added to treeInstances: {tree.name}");

                // Randomize the tree's size
                float randomScale = Random.Range(3f, 7f); // Adjust range as needed
                tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
    }

    private void AddHouseOnLastTile(Path pathGenerator)
    {
        GameObject lastTile = pathGenerator.GetPath()[pathGenerator.GetPath().Count - 1];
        Vector3 housePosition = lastTile.transform.position;
        Instantiate(housePrefab, housePosition, Quaternion.identity);
    }

    private void AddBoundingBox(Path pathGenerator)
    {
        HashSet<Vector3> pathTilePositions = new HashSet<Vector3>();
        foreach (var pathTile in pathGenerator.GetPath())
        {
            pathTilePositions.Add(pathTile.transform.position);
        }

        HashSet<Vector3> boundingBoxPositions = new HashSet<Vector3>();

        foreach (var pathTile in pathGenerator.GetPath())
        {
            Vector3 pathTilePosition = pathTile.transform.position;

            // Check adjacent positions (up, down, left, right)
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(10f, 0, 0), boundingBoxPositions, pathTilePositions); // Right
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(-10f, 0, 0), boundingBoxPositions, pathTilePositions); // Left
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(0, 0, 10f), boundingBoxPositions, pathTilePositions); // Up
            AddBoundingBoxTileAtPosition(pathTilePosition + new Vector3(0, 0, -10f), boundingBoxPositions, pathTilePositions); // Down
        }
    }

    private void AddBoundingBoxTileAtPosition(Vector3 position, HashSet<Vector3> boundingBoxPositions, HashSet<Vector3> pathTilePositions)
    {
        // Check if the position is already occupied by a bounding box or is a path tile
        if (boundingBoxPositions.Contains(position) || pathTilePositions.Contains(position))
            return;

        // Instantiate a bounding box tile at the position
        Instantiate(boundingBoxPrefab, position + new Vector3(0, 1, 0), Quaternion.identity);
        boundingBoxPositions.Add(position);
    }
}