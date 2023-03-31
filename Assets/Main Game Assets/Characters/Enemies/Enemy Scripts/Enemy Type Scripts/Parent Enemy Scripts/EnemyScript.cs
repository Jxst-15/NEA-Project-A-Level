using System;
using UnityEngine;

//[RequireComponent(typeof(EnemyStats))]
//[RequireComponent(typeof(EnemyAI))]
//[RequireComponent(typeof(EnemyCombat))]

public class EnemyScript : MonoBehaviour
{
    #region Script References
    //[SerializeField] private EnemyStats enemyStats;
    //[SerializeField] private EnemyCombat enemyCombat;
    //[SerializeField] private EnemyAI enemyAI;
    #endregion

    private enum EnemyPoints
    {
        NormalEnemy = 1,
        NimbleEnemy = 2,
        BulkyEnemy = 3,
        BossEnemy = 6
    }

    #region Fields
    #region Getters and Setters
    public GameObject target
    { get; set; }
    public string type
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
        type = gameObject.tag;

        // Sets the points depending on the type using the EnemyPoints enum
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
