using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemySpawner : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject thisSpawner;
    public GameObject thisBattleArea;
    #endregion

    #region Script References
    private BattleArea area;
    #endregion

    #region Prefabs
    [SerializeField] GameObject NormalE;
    #endregion

    #region Variables
    private UnityEngine.Vector3 spawnLocation;

    private int enemiesSpawned, enemiesSpawnedInWave, maxToSpawn;
    private int spawnInterval;

    private int waves, wavesDone;
    #endregion

    #region Getters and Setters

    #endregion
    
    private readonly System.Random rng = new System.Random();
    private Dictionary<int, int> wavesMaxSpawn;
    #endregion

    #region Unity Methods
    void Awake()
    {
        thisSpawner = this.gameObject;
        thisBattleArea = this.transform.parent.gameObject;
        
        spawnLocation = thisSpawner.transform.position;
        spawnLocation = new UnityEngine.Vector3(spawnLocation.x, spawnLocation.y, 0);

        enemiesSpawned = 0;
        spawnInterval = 0;

        area = thisBattleArea.GetComponent<BattleArea>();
    }
    
    void OnEnable()
    {
        wavesDone = 1;
        waves = area.waves;
        maxToSpawn = area.eToDefeat / 2;

        Debug.Log("Wave " + wavesDone + " out of " + waves);

        wavesMaxSpawn = new Dictionary<int, int>();

        int count = 1;
        foreach (int i in GetEnemiesInWave(maxToSpawn, waves))
        {
            wavesMaxSpawn.Add(count, i);
            count++;
        }

        StartCoroutine(SpawnEnemy(NormalE, spawnInterval));

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        CheckWave(NormalE, spawnInterval);
    }
    #endregion

    private void CheckEnemiesSpawned(GameObject enemy, int interval)
    {
        if (enemiesSpawnedInWave != wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            StartCoroutine(SpawnEnemy(enemy, interval));
        }
        else if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            Debug.Log("All enemies spawned for this wave");
            StopCoroutine(SpawnEnemy(enemy, interval));
            // CheckWave(enemy, interval);
        }
    }

    private void CheckWave(GameObject enemy, int interval)
    {
        if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            if (wavesDone != waves && area.enemies.Count == 0)
            {
                StartNewWave(enemy, interval);
            }
            else if (wavesDone == waves && area.enemies.Count == 0)
            {
                Debug.Log("Area Cleared");
                StopCoroutine(SpawnEnemy(enemy, interval));
                area.areaCleared = true;
            }
        }
    }

    private void StartNewWave(GameObject enemy, int interval)
    {
        wavesDone++;
        Debug.Log("New Wave: Wave " + wavesDone);
        enemiesSpawned = enemiesSpawnedInWave;
        enemiesSpawnedInWave = 0;


        StartCoroutine(SpawnEnemy(enemy, interval));
    }

    public static IEnumerable<int> GetEnemiesInWave(int maxToSpawn, int waves)
    {
        int remainder;
        int result = Math.DivRem(maxToSpawn, waves, out remainder);

        for (int i = 0; i < waves; i++)
        {
            yield return i < remainder ? result + 1 : result;
        }
    }

    private IEnumerator SpawnEnemy(GameObject toSpawn, int interval)
    {
        spawnInterval = rng.Next(5, 9);
        
        yield return new WaitForSeconds(interval);
        
        GameObject enemy = Instantiate(toSpawn, spawnLocation, UnityEngine.Quaternion.identity);

        enemiesSpawnedInWave++;
        interval = spawnInterval;

        CheckEnemiesSpawned(enemy, interval);
    }
}
