using UnityEngine;

public class EnemyMovement : CharMovement
{
    #region Fields
    #region Script References
    [SerializeField] protected EnemyScript enemyScript;
    [SerializeField] protected EnemyStats enemyStats;
    #endregion

    #region Target
    protected Transform targetPos;
    #endregion

    #region Variables
    protected const bool notNeeded = false;

    protected float enemyScaleX, enemyScaleY;
    protected float distanceFromTarget;

    protected float maxTrackDistance;
    protected float runDistance;
    protected float attackDistance;
    #endregion

    #region Getters and Setters
    public bool inRange
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        enemyStats = GetComponent<EnemyStats>();
        
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            if (canMove == true)
            {
                enemyScaleX = this.transform.localScale.x;
                enemyScaleY = this.transform.localScale.y;

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

        vSpeed = enemyStats.vSpeed;
        hSpeed = enemyStats.hSpeed;
        vRunSpeed = enemyStats.vRunSpeed;
        hRunSpeed = enemyStats.hRunSpeed;

        canMove = true;
        isRunning = false;

        isDodging = false;
        startDodgeTime = 0.1f;
        dodgeTime = startDodgeTime;
        dodgeSpeed = 35;
        side = 0;

        maxTrackDistance = 15f;
        runDistance = 8f;
        attackDistance = 2.5f;

        targetPos = enemyScript.target.transform;
    }

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
        else if (distanceFromTarget <= attackDistance)
        {
            return 2;
        }

        return -1;
    }

    protected override void Movement()
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
                // rb.velocity = new Vector2(distanceFromTarget * hSpeed, distanceFromTarget * vSpeed);
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

    protected override void Run()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, hRunSpeed * Time.deltaTime);
        // rb.velocity = new Vector2(distanceFromTarget * hRunSpeed, distanceFromTarget * vRunSpeed);
    }

    protected override void Dodge()
    {
        Debug.Log("Dodge");
    }

    protected override void Action()
    {
        Debug.Log("Action");
    }

    // Needs fixing
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
