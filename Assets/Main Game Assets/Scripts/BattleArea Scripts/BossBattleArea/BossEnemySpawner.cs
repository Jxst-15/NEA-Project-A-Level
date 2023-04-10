using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : BattleAreaEnemySpawner
{
    #region Fields
    #region Objects
    private List<Collider2D> enemiesInArea;
    #endregion

    #region Gameobjects
    public GameObject bossEnemy;
    #endregion
    #endregion

    protected override void SetAreaMinAndMax()
    {
        area.min = 4;
        area.max = 7;
    }

    protected override void SetWavesAndMaxSpawn()
    {
        base.SetWavesAndMaxSpawn();
        maxToSpawn *= 2;
    }

    protected override void PopulateQueue(int max)
    {
        for (int i = 0; i < maxToSpawn - 1; i++)
        {
            enemies.Enqueue(ChooseEnemy());
        }
        enemies.Enqueue(bossEnemy);
    }
}
