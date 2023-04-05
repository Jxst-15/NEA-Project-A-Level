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

    public GameObject[] enemyPrefabs = new GameObject[3];
    protected MyQueue<GameObject> enemies = new MyQueue<GameObject>();
    #endregion

    #region Gameobjects
    protected GameObject thisSpawner;
    #endregion

    #region Variables
    protected Vector3 spawnLocation;

    protected int enemiesSpawned, maxToSpawn;
    
    protected int spawnInterval;
    protected int minInterval, maxInterval;
    #endregion

    public Material normalMaterial;

    #region Getters and Setters
    public bool spawning
    { get; protected set; }
    #endregion
    #endregion

    #region Unity Methods
    // Sets the necessary values to attributes
    protected virtual void Awake()
    {
        thisSpawner = this.gameObject;

        spawnLocation = thisSpawner.transform.position;
        spawnLocation = new Vector3(spawnLocation.x, spawnLocation.y, 0);

        spawnInterval = 0;
        minInterval = 4;
        maxInterval = 8;
    }

    protected virtual void OnEnable()
    {
        SetWavesAndMaxSpawn();

        PopulateQueue(maxToSpawn);

        Debug.Log(maxToSpawn);
    }

    // Update is called once per frame
    protected virtual void Update() { }
    #endregion

    protected virtual void SetWavesAndMaxSpawn()
    {
        // maxWave = rng.Next(2, 4);

        int eToDefeat = rng.Next(4 / 2, 7 / 2) * 2;
        maxToSpawn = eToDefeat;
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

        GameObject enemy = enemyPrefabs[index];

        return enemy;
    }

    // Stops or starts the SpawnEnemy IEnumerator depending on if all enemies have been spawned
    protected virtual void CheckEnemiesSpawned(int interval)
    {
        if (enemiesSpawned != maxToSpawn)
        {
            Debug.Log(enemies.Size() + " " + enemiesSpawned);
            StartCoroutine(SpawnEnemy(interval));
        }
        else
        {
            Debug.Log("All enemies spawned");
            spawning = false;
            StopCoroutine(SpawnEnemy(interval));
            
            Destroy(gameObject);
        }
    }

    protected virtual void UpdateEnemiesSpawned()
    {
        enemiesSpawned++;
    }

    protected IEnumerator SpawnEnemy(int interval)
    {
        if (spawning == true)
        {
            spawnInterval = rng.Next(minInterval, maxInterval); // Randomises the spawn time

            yield return new WaitForSeconds(interval);

            GameObject enemy = Instantiate(enemies.Dequeue(), spawnLocation, Quaternion.identity); // Creates the enemy gameobject at the given position with no rotation

            enemy.GetComponent<SpriteRenderer>().material = normalMaterial; // Sets the material to default to prevent enemies which have the flash materials on
            // enemy.name = toSpawn.name + " " + enemiesSpawned; // Adjusts the name of the enemy spawned

            UpdateEnemiesSpawned();
            interval = spawnInterval;

            CheckEnemiesSpawned(interval);
        }
    }

    // Activates the spawner if the player has entered the trigger area
    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            spawning = true;
            StartCoroutine(SpawnEnemy(0));
        }
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == "Player")
        {
            spawning = false;
            StopCoroutine(SpawnEnemy(spawnInterval));
        }
    }
    #endregion
}
