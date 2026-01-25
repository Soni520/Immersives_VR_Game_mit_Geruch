using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class spawn_logic : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToSpawn;
    [SerializeField] private Vector3[] spawnPositions;


    public GameObject[] ObjectsToSpawn => objectsToSpawn;

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Erstelle eine Liste der verfügbaren Positionen
        List<Vector3> availablePositions = new List<Vector3>(spawnPositions);
        
        // Mische die Positionen zufällig
        availablePositions = availablePositions.OrderBy(x => Random.value).ToList();
        
        // Spawne jedes Objekt an einer zufälligen, noch nicht verwendeten Position
        int spawnCount = Mathf.Min(objectsToSpawn.Length, availablePositions.Count);
        
        for (int i = 0; i < spawnCount; i++)
        {
            Instantiate(objectsToSpawn[i], availablePositions[i], Quaternion.identity);
        }
    }

    void Update()
    {
        
    }
}