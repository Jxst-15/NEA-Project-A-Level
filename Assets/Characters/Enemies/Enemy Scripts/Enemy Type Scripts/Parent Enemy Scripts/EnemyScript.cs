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

    public PlayerCombat targetAttackStatus;
    public PlayerStats targetStats;
    public PlayerBlock targetBlockStats;
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

        this.enemyStats = GetComponent<EnemyStats>();
        this.enemyAI = GetComponent<EnemyAI>();
        this.enemyCombat = GetComponent<EnemyCombat>();

        this.targetAttackStatus = target.GetComponent<PlayerCombat>();
        this.targetStats = target.GetComponent<PlayerStats>();
        this.targetBlockStats = target.GetComponentInChildren<PlayerBlock>();

        // Sets the enemy type to the tag which is given in the editor
        type = gameObject.tag;
    }
}
