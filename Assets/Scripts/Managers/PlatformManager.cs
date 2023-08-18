using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    /// <summary>
    /// THIS CS. IS NOT FOR THIS GAME BUT CAN BE USED IN ANYOTHER RUNNER
    /// </summary>
    [Header("Hat")]
    [SerializeField] GameObject  hatPrefab;
    [SerializeField] List<GameObject> hatSpawnLocations;

    [Header("Obstacle")]

    [SerializeField] List<GameObject>  obstaclePrefabList;
    [SerializeField] List<GameObject> obstacleSpawnLocations;

    [Header("Gate")]
    [SerializeField] GameObject  gatePrefab;
    [SerializeField] List<GameObject> gateSpawnLocations;
    GameObject spawnedObject;

    void Start()
    {
        SpawnObjects(hatPrefab,hatSpawnLocations, Quaternion.Euler(0,90,0), 80);

        SpawnObstacle(50);

        SpawnObjects(gatePrefab, gateSpawnLocations, Quaternion.Euler(0,0,0),75);
    }

    void SpawnObjects(GameObject prefab, List<GameObject> spawnLocationList, Quaternion rotation,int spawnChance)
    {
        for (int i = 0; i < spawnLocationList.Count; i++)
        {    
            int spawnRand = Random.Range(0,99);
            if(spawnRand < spawnChance)
            {
                int LocationRand = Random.Range (0,spawnLocationList.Count);
                spawnedObject =  Instantiate(prefab,
                spawnLocationList[LocationRand].transform.position
                ,rotation);
                spawnLocationList.RemoveAt(LocationRand);
            }
        }
    }

    void SpawnObstacle(int spawnChance)
    {
        for (int i = 0; i < obstacleSpawnLocations.Count; i++)
        {    
            int spawnRand = Random.Range(0,99);
            if(spawnRand < spawnChance)
            {
                int LocationRand = Random.Range (0,obstacleSpawnLocations.Count);
                int prefabRand = Random.Range (0,obstaclePrefabList.Count);
                spawnedObject =  Instantiate(obstaclePrefabList[prefabRand],
                obstacleSpawnLocations[LocationRand].transform.position
                ,obstacleSpawnLocations[LocationRand].transform.position.x > 0 ? 
                Quaternion.Euler(0,0,0) : Quaternion.Euler(0,180,0));
                obstacleSpawnLocations.RemoveAt(LocationRand);
            }
        }
    }
}
