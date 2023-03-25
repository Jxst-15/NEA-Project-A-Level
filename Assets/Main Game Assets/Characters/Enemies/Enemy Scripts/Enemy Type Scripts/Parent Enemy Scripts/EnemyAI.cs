using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Old
    /*
    // Determining the distance between this object and the player
    private int TrackPlayer(float distanceFromTarget)
    {
        if (distanceFromTarget >= runDistance && distanceFromTarget < maxTrackDistance)
        {
            return 0;
        }
        else if (distanceFromTarget < runDistance && distanceFromTarget > attackDistance)
        {
            return 1;
        }
        else if (distanceFromTarget  <= attackDistance)
        {
            return 2;
        }
        
        return -1;
    }
    
    // Actually moving this object towards the target
    public virtual void Movement()
    {
        if (canMove == true)
        {
            distanceFromTarget = Vector2.Distance(transform.position, targetPos.position);
            switch (TrackPlayer(distanceFromTarget))
            {
                case 0:
                    // Make enemy run towards player
                    inRange = false;
                    isRunning = true;
                    Run();
                    break;
                case 1:
                    // Make enemy walk towards player
                    inRange = false;
                    isRunning = false;
                    transform.position = Vector2.MoveTowards(transform.position, targetPos.position, hSpeed * Time.deltaTime);
                    break;
                case 2:
                    // Enemy in attack range, make enemy attack and stop moving
                    inRange = true;
                    isRunning = false;
                    // Also random chance of dodging whenever player attacks
                    break;
                case -1:
                    // Enemy will stand still waiting for player to get in range
                    inRange = false;
                    isRunning = false;
                    break;
            }
        }
    }
    */
    #endregion

    #region Fields
    #region FSM
    public EnemyAIFSM fsm;
    #endregion

    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyCombat enemyCombat;
    #endregion

    #region Script Reference Variables
    private Transform targetPos;
    #endregion

    #endregion

    #region Unity Methods
    private void Start()
    {
        // Creates the EnemyAIFSM
        fsm = new EnemyAIFSM();

        enemyScript = GetComponent<EnemyScript>();
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<EnemyCombat>();
        
        // Enemy has been made active and is spawned, set state to idle
        fsm.MoveStates(EnemyCommands.Spawned);

        targetPos = enemyScript.target.transform;
    }

    private void FixedUpdate()
    {
        if (fsm.currentState.acceptingState != true)
        {
            fsm.distanceFromTarget = Vector2.Distance(transform.position, targetPos.position);
            
            // Calls the UpdateFixed in order to determine which state the enemy is in
            fsm.currentState.UpdateFixed();
        }
    }
    #endregion
}
