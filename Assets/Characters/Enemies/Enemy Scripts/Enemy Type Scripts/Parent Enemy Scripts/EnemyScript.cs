using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(EnemyCombat))]

public class EnemyScript : MonoBehaviour
{
    #region Script References
    protected EnemyStats enemyStats;
    protected EnemyAI enemyAI;
    protected EnemyCombat enemyCombat;
    #endregion

    #region Variables 
    protected GameObject target;

    protected string type;
    #endregion

    #region Getters and Setters
    [SerializeField] public float points
    { get; set; }
    [SerializeField] public int scoreToGive
    { get ; set; }
    #endregion

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        enemyStats = GetComponent<EnemyStats>();
        enemyAI = GetComponent<EnemyAI>();
        enemyCombat = GetComponent<EnemyCombat>();
        
        // Sets the enemy type to the tag which is given in the editor
        type = gameObject.tag;
    }
}
