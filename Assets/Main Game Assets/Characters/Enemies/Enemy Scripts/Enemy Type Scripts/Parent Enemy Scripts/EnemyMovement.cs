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
    protected const bool notNeeded = false;

    protected float enemyScaleX, enemyScaleY;

    protected Vector2 direction;

    protected float runDistance;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAI = GetComponent<EnemyAI>();
        
        base.Start();
        enemyScaleX = this.transform.localScale.x;
        enemyScaleY = this.transform.localScale.y;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            if (canMove == true)
            {
                Flip(notNeeded, enemyScaleX, enemyScaleY);
            }
            // Action();
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

                // Dodge();
            }
        }
    }
    #endregion

    protected override void SetVariables()
    {
        base.SetVariables();

        canMove = true;
        isRunning = false;

        isDodging = false;
        startDodgeTime = 0.1f;
        dodgeTime = startDodgeTime;
        dodgeSpeed = 35;
        side = 0;

        runDistance = 8f;

        targetPos = enemyScript.target.transform;
    }

    protected override void Movement()
    {
        direction = (targetPos.position - transform.position).normalized;

        if (enemyAI.fsm.currentState.thisStateID == EnemyStates.Tracking)
        {
            if (enemyAI.fsm.distanceFromTarget >= runDistance)
            {
                Run();
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

    protected override void Run()
    {
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
