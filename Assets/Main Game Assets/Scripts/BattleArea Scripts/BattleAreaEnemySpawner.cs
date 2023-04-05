using System.Linq;
using UnityEngine;

public class BattleAreaEnemySpawner : EnemySpawner
{
    #region Fields
    #region Gameobjects
    private GameObject thisBattleArea;
    #endregion

    #region Script References
    private BattleArea area;
    #endregion
    #endregion

    #region Unity Methods
    // Sets the necessary values to attributes
    protected override void Awake()
    {
        base.Awake();

        thisBattleArea = this.transform.parent.gameObject;
        area = thisBattleArea.GetComponent<BattleArea>();
    }
    #endregion

    protected override void SetWavesAndMaxSpawn()
    {
        maxWave = area.waves;
        maxToSpawn = area.eToDefeat / 2;
    }

    // Checks to see if the wave is the last wave or not
    protected override void CheckWave(int interval)
    {
        if (enemiesSpawnedInWave == wavesMaxSpawn.ElementAt(wavesDone - 1).Value)
        {
            if (wavesDone != maxWave && area.enemies.Count == 0)
            {
                StartNewWave(interval);
            }
            else if (wavesDone == maxWave && area.enemies.Count == 0)
            {
                Debug.Log("Area Cleared");
                StopCoroutine(SpawnEnemy(enemies.Dequeue(), interval));
                area.areaCleared = true;
            }
        }
    }
}
