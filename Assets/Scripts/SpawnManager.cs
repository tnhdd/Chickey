using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnData
{

    public GameObject prefab;
    public float spawnRateMin;
    public float spawnRateMax;

    public float startDelay;
    public int maxSpawn; // Add a field for max spawn count
    public bool randomScale;
    public float minScale, maxScale;
}

public class SpawnManager : MonoBehaviour
{
    [SerializeField] SpawnData[] objectsToSpawnData;
    [SerializeField] float range;

    int totalSpawnCount = 0;
    float spawnRate;

    void Start()
    {
        foreach (SpawnData data in objectsToSpawnData)
        {
            StartCoroutine(SpawnObjects(data));
        }
    }

   
    IEnumerator SpawnObjects(SpawnData data)
    {
        yield return new WaitForSeconds(data.startDelay);

        while (totalSpawnCount < data.maxSpawn)
        {
            Vector3 spawnPosition = new Vector3(UnityEngine.Random.Range(-range, range), 0f, UnityEngine.Random.Range(-range, range));
            Quaternion randomRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

            GameObject spawnedObject = Instantiate(data.prefab, spawnPosition, randomRotation);

            if (data.randomScale == true)
            {
                float randomScale = Random.Range(data.minScale, data.maxScale);
                spawnedObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
            }
            totalSpawnCount++;
            spawnRate = UnityEngine.Random.Range(data.spawnRateMin, data.spawnRateMax);
            yield return new WaitForSeconds(spawnRate);
        }
    }

    int CountGameObjectsWithName(string name)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == name)
            {
                count++;
            }
        }

        return count;
    }
}
