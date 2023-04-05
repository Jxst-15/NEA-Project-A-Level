using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : EnemySpawner
{
    #region Fields
    #region Gameobjects
    public GameObject bossEnemy;
    #endregion
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        minInterval = 6;
        maxInterval = 12;
    }
    #endregion

    protected override void PopulateQueue(int max)
    {
        for (int i = 0; i < maxToSpawn - 1; i++)
        {
            enemies.Enqueue(ChooseEnemy());
        }
        enemies.Enqueue(bossEnemy);
    }

    protected override void CheckEnemiesSpawned(int interval)
    {
        if (enemiesSpawned != maxToSpawn)
        {
            Debug.Log(enemies.Size() + " " + enemiesSpawned);
            StartCoroutine(SpawnEnemy(interval));
        }
        else if (enemiesSpawned == maxToSpawn - 1)
        {
            StartCoroutine(SpawnEnemy(9));
        }
        else
        {
            Debug.Log("All enemies spawned");
            spawning = false;
            StopCoroutine(SpawnEnemy(interval));

            Destroy(gameObject);
        }
    }
}
