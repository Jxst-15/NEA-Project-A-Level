using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Fields
    #region FSM
    public EnemyAIFSM fsm;
    #endregion

    #region Script References
    [SerializeField] private EnemyScript enemyScript;
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
