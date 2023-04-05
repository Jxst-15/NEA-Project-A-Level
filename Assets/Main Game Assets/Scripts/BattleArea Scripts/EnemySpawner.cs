using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Fields
    #region Object References
    protected readonly System.Random rng = new System.Random();

    protected Dictionary<int, int> wavesMaxSpawn;

    public GameObject[] enemyPrefabs = new GameObject[3];
    protected MyQueue<GameObject> enemies = new MyQueue<GameObject>();
    #endregion

    #region Gameobjects
    protected GameObject thisSpawner;
    #endregion

    #region Variables
    protected Vector3 spawnLocation;

    protected int enemiesSpawnedInWave, maxToSpawn;
    protected int spawnInterval;

    protected int maxWave, wavesDone;

    protected bool areaCleared;
    #endregion

    public Material normalMaterial;
    #endregion

    #region Unity Methods
    // Sets the necessary values to attributes
    protected virtual void Awake()
    {
        thisSpawner = this.gameObject;

        spawnLocation = thisSpawner.transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, 0);

        spawnInterval = 0;
    }

    protected void OnEnable()
    {
        wavesDone = 1;
        SetWavesAndMaxSpawn();

        Debug.Log("Wave " + wavesDone + " out of " + maxWave);

        wavesMaxSpawn = new Dictionary<int, int>();

        int count = 1;

        // Populates the wavesMaxSpawn dictionary with the number of waves and the enemies to spawn
        foreach (int i in GetEnemiesInWave(maxToSpawn, maxWave))
        {
            wavesMaxSpawn.Add(count, i);
            count++;
        }

        PopulateQueue(wavesMaxSpawn.ElementAt(wavesDone).Value);

        StartCoroutine(SpawnEnemy(enemies.Dequeue(), spawnInterval));

    }

    // Update is called once per frame
    protected void Update()
    {
        CheckWave(spawnInterval);
    }
    #endregion

    protected virtual void SetWavesAndMaxSpawn()
    {
        maxWave = rng.Next(2,4);
        
        int eToDefeat = rng.Next(8 / 2, 11 / 2) * 2;
        maxToSpawn = eToDefeat / 2;
    }

    // Fills the queue with enemy gameobjects
    protected void PopulateQueue(int max)
    {
        for (int i = 0; i < maxToSpawn; i++)
        {
            enemies.Enqueue(ChooseEnemy());
        }
    }

    // Returns a random enemy prefab from the enemyPrefabs array
    protected GameObject ChooseEnemy()
    {
        int rand = rng.Next(1, 7);
        int index;
        if (1 <= rand && rand <= 3)
        {
            index = 1;
        }
        else if (rand == 4 || rand == 5)
        {
            index = 0;
        }
        else
        {
            index = 2;
        }

        // int index = rng.Next(0, enemyPrefabs.Length);
        GameObject enemy = enemyPrefabs[index];

        return enemy;
    }

    // Stops or starts the SpawnEnemy IEnumerator depending on if all enemies have been spawned
    protected void CheckEnemiesSpawned(int interval)
    {
        if (enemiesSpawnedInWave != wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            StartCoroutine(SpawnEnemy(enemies.Dequeue(), interval));
        }
        else if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            Debug.Log("All enemies spawned for this wave");
            StopCoroutine(SpawnEnemy(enemies.Dequeue(), interval));
        }
    }

    // Checks to see if the wave is the last wave or not
    protected virtual void CheckWave(int interval)
    {
        if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            if (wavesDone != maxWave)
            {
                StartNewWave(interval);
            }
            else
            {
                Debug.Log("All enemies spawned");
                StopCoroutine(SpawnEnemy(enemies.Dequeue(), interval));
            }
        }
    }

    protected void StartNewWave(int interval)
    {
        wavesDone++;
        Debug.Log("New Wave: Wave " + wavesDone);
        enemiesSpawnedInWave = 0;

        PopulateQueue(wavesMaxSpawn.ElementAt(wavesDone - 1).Value); // Repopulates the queue with enemies

        StartCoroutine(SpawnEnemy(enemies.Dequeue(), interval));
    }

    // Splits up the amount of enemies to spawn in the wave based on the maxToSpawn variable
    protected IEnumerable<int> GetEnemiesInWave(int maxToSpawn, int waves)
    {
        int remainder;
        int result = Math.DivRem(maxToSpawn, waves, out remainder); // Calculates the result and the remainder 

        for (int i = 0; i < waves; i++)
        {
            yield return i < remainder ? result + 1 : result; // Returns either the result + 1 if i < remainder is true, else output just the result
        }
    }

    protected IEnumerator SpawnEnemy(GameObject toSpawn, int interval)
    {
        spawnInterval = rng.Next(6, 12); // Randomises the spawn time

        yield return new WaitForSeconds(interval);

        GameObject enemy = Instantiate(toSpawn, spawnLocation, Quaternion.identity); // Creates the enemy gameobject at the given position with no rotation

        enemy.GetComponent<SpriteRenderer>().material = normalMaterial; // Sets the material to default to prevent enemies which have the flash materials on
        enemy.name = toSpawn.name + " " + enemiesSpawnedInWave; // Adjusts the name of the enemy spawned

        enemiesSpawnedInWave++;
        interval = spawnInterval;

        CheckEnemiesSpawned(interval);
    }
}
