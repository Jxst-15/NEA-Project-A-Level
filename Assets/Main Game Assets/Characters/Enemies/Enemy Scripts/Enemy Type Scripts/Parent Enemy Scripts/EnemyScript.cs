using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Denotes how many points should be given depending on the tag of enemy
    private enum EnemyPoints
    {
        NormalEnemy = 3,
        NimbleEnemy = 2,
        BulkyEnemy = 4,
        BossEnemy = 8
    }

    #region Fields
    #region Getters and Setters
    public GameObject target
    { get; set; }
    public int points
    { get; set; }
    public int scoreToGive
    { get ; set; }
    #endregion
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Sets the enemy type to the tag which is given in the editor
        string type = gameObject.tag;

        // Sets the points depending on the type (tag) using the EnemyPoints enum
        switch(type)
        {
            case "NormalEnemies":
                points = Convert.ToInt32(EnemyPoints.NormalEnemy);
                break;
            case "NimbleEnemies":
                points = Convert.ToInt32(EnemyPoints.NimbleEnemy);
                break;
            case "BulkyEnemies":
                points = Convert.ToInt32(EnemyPoints.BulkyEnemy);
                break;
            case "BossEnemies":
                points = Convert.ToInt32(EnemyPoints.BossEnemy);
                break;
        }
        
        target = GameObject.FindGameObjectWithTag("Player");
    }
    #endregion

    public void OnDeath()
    {
        // Give player points
        target.GetComponent<PlayerPoints>().ChangePoints(points, "inc");
        PlayerData.instance.enemiesDefeated++;
    }
}
