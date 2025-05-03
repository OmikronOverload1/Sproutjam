using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public static List<GameObject> GeneratedTiles = new List<GameObject>();

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject pathTilePrefab; // Prefab for Path tiles
    [SerializeField] private GameObject playerPrefab;   // Prefab for the player
    [SerializeField] private GameObject[] treePrefabs;  // Array of tree prefabs

    private int radius = 20;

    void Start()
    {
        Path pathGenerator = new Path(radius);

        for (int x = 0; x < radius; x++)
        {
            for (int z = 0; z < radius; z++)
            {
                GameObject tile = Instantiate(tilePrefab,
                    new Vector3(x * 20f, 0, z * 20f), Quaternion.identity);

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
            int treeCount = Random.Range(2, 6);

            for (int i = 0; i < treeCount; i++)
            {
                // Randomly select a tree prefab
                GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];

                Vector3 randomOffset = new Vector3(
                    Random.Range(-3f, 3f), 
                    -0.4f,                    
                    Random.Range(-3f, 3f) 
                );
                Vector3 treePosition = tile.transform.position + randomOffset;

                // Instantiate the tree
                GameObject tree = Instantiate(treePrefab, treePosition, Quaternion.identity);

                // Randomize the tree's size
                float randomScale = Random.Range(1f, 8.5f); // Adjust range as needed
                tree.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
        }
    }
}