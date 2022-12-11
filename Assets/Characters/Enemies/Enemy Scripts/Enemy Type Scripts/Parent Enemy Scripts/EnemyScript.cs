using UnityEngine;

//[RequireComponent(typeof(EnemyStats))]
//[RequireComponent(typeof(EnemyAI))]
//[RequireComponent(typeof(EnemyCombat))]

public class EnemyScript : MonoBehaviour
{
    #region Script References
    private EnemyStats enemyStats;
    private EnemyCombat enemyCombat;
    private EnemyAI enemyAI;
    #endregion

    #region Getters and Setters
    public GameObject target
    { get; set; }
    public string type
    { get; set; }
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

    private void Update()
    {
        enemyStats.EStatsUpdate();
        enemyAI.EAIUpdate();
        enemyCombat.ECombatUpdate();
    }

    private void FixedUpdate()
    {
        enemyAI.Movement();
    }
}
