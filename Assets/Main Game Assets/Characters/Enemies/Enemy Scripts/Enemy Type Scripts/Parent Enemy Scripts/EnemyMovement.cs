using UnityEngine;

public class EnemyMovement : CharMovement
{
    #region Fields
    #region Script References
    [SerializeField] protected EnemyScript enemyScript;
    [SerializeField] protected EnemyStats enemyStats;
    [SerializeField] protected EnemyAI enemyAI;
    #endregion

    #region Target
    protected Transform targetPos;
    #endregion

    #region Variables
    protected float runDistance;
    #endregion
    
    protected const bool notNeeded = false;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAI = GetComponent<EnemyAI>();
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            if (canMove == true)
            {
                Flip(notNeeded, scaleX, scaleY);
            }
            // Action(); // Where the action method would go
        }
    }

    protected override void FixedUpdate()
    {
        if (PauseMenu.isPaused == false)
        {
            isRunning = false;
            isDodging = false;
            if (canMove == true)
            {
                if (isDodging == false || isRunning == false)
                {
                    Movement();
                }

                // Dodge(); // Where the dodge method would go
            }
        }
    }
    #endregion

    protected override void SetVariables()
    {
        base.SetVariables();

        startDodgeTime = 0.1f;
        dodgeTime = startDodgeTime;
        dodgeSpeed = 35;
        side = 0;

        runDistance = 8f; // If distance is less than or equal this than will walk, else then run

        targetPos = enemyScript.target.transform; // The position of the target
    }

    protected override void Movement()
    {
        Vector2 direction = (targetPos.position - transform.position).normalized;

        // IF the enemy state is in tracking
        if (enemyAI.fsm.currentState.thisStateID == EnemyStates.Tracking)
        {
            if (enemyAI.fsm.distanceFromTarget >= runDistance)
            {
                Run(direction);
            }
            else if (enemyAI.fsm.distanceFromTarget < runDistance)
            {
                rb.velocity = new Vector2(direction.x * enemyStats.hSpeed, direction.y * enemyStats.vSpeed);
            }
        }
        else
        {
            StopMovement();
        }
    }

    protected override void Run(Vector2 direction)
    {
        // Manipulates the velocity of the rigidbody
        rb.velocity = new Vector2(direction.x * enemyStats.hRunSpeed, direction.y * enemyStats.vRunSpeed);
    }

    protected override void Dodge()
    {
        Debug.Log("Dodge");
    }

    protected override void Action()
    {
        Debug.Log("Action");
    }

    protected override void Flip(bool value, float scaleX, float scaleY)
    {
        if (transform.position.x > targetPos.position.x)
        {
            this.transform.localScale = new Vector2(scaleX, scaleY);
        }
        else
        {
            this.transform.localScale = new Vector2(-scaleX, scaleY);
        }
    }

}
