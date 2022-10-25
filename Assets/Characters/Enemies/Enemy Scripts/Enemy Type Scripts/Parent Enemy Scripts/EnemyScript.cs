using UnityEngine;

[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EnemyAI))]
// [RequireComponent(typeof(EnemyCombat))]

public class EnemyScript : MonoBehaviour
{
    #region Script References
    EnemyStats enemyStats;
    EnemyAI enemyAI;
    // EnemyCombat enemyCombat;
    #endregion

    #region Variables 
    protected GameObject target;
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
        // enemyCombat = GetComponent<EnemyCombat>();
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       
    }
}
