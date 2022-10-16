using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyAI))]

public class EnemyScript : MonoBehaviour
{
    #region Script References
    EnemyStats enemyStats;
    EnemyCombat enemyCombat;
    EnemyAI enemyAI;
    #endregion

    #region Variables 
    GameObject target;
    #endregion

    #region Getters and Setters
    [SerializeField] private float points
    { get; set; }
    [SerializeField] private int scoreToGive
    { get ; set; }
    #endregion


    private void Awake()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyCombat = GetComponent<EnemyCombat>();
        enemyAI = GetComponent<EnemyAI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
