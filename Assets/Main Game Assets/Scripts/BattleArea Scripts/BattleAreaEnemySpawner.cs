using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleAreaEnemySpawner : EnemySpawner
{
    #region Fields
    #region Gameobjects
    protected GameObject thisBattleArea;
    #endregion

    #region Object References
    [SerializeField] protected Dictionary<int, int> wavesMaxSpawn;
    #endregion

    #region Script References
    protected BattleArea area;
    #endregion

    #region Variables
    protected int enemiesSpawnedInWave;
    protected int maxWave, wavesDone;
    #endregion
    #endregion

    #region Unity Methods
    // Sets the necessary values to attributes
    protected override void Awake()
    {
        base.Awake();

        thisBattleArea = this.transform.parent.gameObject;
        area = thisBattleArea.GetComponent<BattleArea>();
        spawning = true;

        minInterval = 6;
        maxInterval = 12;
    }

    protected override void OnEnable()
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

        StartCoroutine(SpawnEnemy(spawnInterval));
    }

    // Update is called once per frame
    protected override void Update()
    {
        CheckWave(spawnInterval);
    }
    #endregion

    protected override void SetWavesAndMaxSpawn()
    {
        maxWave = area.waves;
        maxToSpawn = area.eToDefeat / 2;
    }

    protected override void CheckEnemiesSpawned(int interval)
    {
        if (enemiesSpawnedInWave != wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            StartCoroutine(SpawnEnemy(interval));
        }
        else
        {
            Debug.Log("All enemies spawned for this wave");
            StopCoroutine(SpawnEnemy(interval));
        }
    }

    // Checks to see if the wave is the last wave or not
    protected virtual void CheckWave(int interval)
    {
        if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            if (wavesDone != maxWave && area.enemies.Count == 0)
            {
                StartNewWave(interval);
            }
            else if (wavesDone == maxWave && area.enemies.Count == 0)
            {
                StopCoroutine(SpawnEnemy(interval));
                spawning = false;
            }
        }
    }

    protected void StartNewWave(int interval)
    {
        wavesDone++;
        Debug.Log("New Wave: Wave " + wavesDone);
        enemiesSpawnedInWave = 0;

        PopulateQueue(wavesMaxSpawn.ElementAt(wavesDone - 1).Value); // Repopulates the queue with enemies

        StartCoroutine(SpawnEnemy(interval));
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

    protected override void UpdateEnemiesSpawned()
    {
        enemiesSpawnedInWave++;
    }
}
