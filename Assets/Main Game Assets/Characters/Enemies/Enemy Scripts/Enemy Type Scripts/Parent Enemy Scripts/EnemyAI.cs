using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    #region Old
    /*
    #region Fields
    #region FSM
    private EnemyFSM fsm;
    #endregion

    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyStats enemyStats;
    #endregion

    #region Script Reference Variables
    [SerializeField] private int vSpeed, hSpeed, vRunSpeed, hRunSpeed;
    #endregion

    #region Target
    private Transform targetPos;
    #endregion
    
    #region Variables
    private const bool notNeeded = false;

    [SerializeField] private bool isRunning;
    private float enemyScaleX, enemyScaleY;
    private float distanceFromTarget;

    [SerializeField] private bool isDodging;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side;
    private float dodgeTime;

    private float maxTrackDistance;
    private float runDistance;
    private float attackDistance;

    public Rigidbody2D rb;
    #endregion

    #region Getters and Setters
    public bool canMove
    { get; set; }
    [SerializeField] public bool inRange
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyStats = GetComponent<EnemyStats>();
        enemyScript = GetComponent<EnemyScript>();
        SetVariables();
        
        // Finds the target by getting the target variable from the EnemyScript parent class and gets the transform component 
        targetPos = enemyScript.target.transform;

        fsm = new EnemyFSM();
    }
   
    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused == false || canMove == false)
        {
            EAIUpdate();

        }
    }

    void FixedUpdate()
    {
        if (PauseMenu.isPaused == false || canMove == false)
        {
            Movement();
        }
    }
    #endregion

    private void SetVariables()
    {
        vSpeed = enemyStats.vSpeed;
        hSpeed = enemyStats.hSpeed;
        vRunSpeed = enemyStats.vRunSpeed;
        hRunSpeed = enemyStats.hRunSpeed;

        canMove = true;
        isRunning = false;

        isDodging = false;
        dodgeSpeed = 35;
        side = 0;

        maxTrackDistance = 15f;
        runDistance = 8f;
        attackDistance = 2.5f;

        enemyScaleX = this.transform.localScale.x;
        enemyScaleY = this.transform.localScale.y;
    }

    public void EAIUpdate()
    {
        if (canMove == true)
        {

            // Run action method

            // For flipping the sprite
            Flip(notNeeded, enemyScaleX, enemyScaleY);
        }
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

    // Method to make enemy run towards player
    public void Run()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos.position, hRunSpeed * Time.deltaTime);
    }

    // Method to make enemy randomly dodge when player is attacking 
    public void Dodge()
    {

    }

    // Method to make enemy randomly pick up item whenever near one
    public void Action()
    {

    }

    // Flips game object depending on the direction the player is
    public void Flip(bool notNeeded, float scaleX, float scaleY)
    {
        if (transform.position.x > targetPos.position.x)
        {
            // this.transform.localScale = new Vector2(-1.89751f, scaleY);
            this.transform.localScale = new Vector2(scaleX, scaleY);
        }
        else
        {
            // this.transform.localScale = new Vector2(1.89751f, scaleY);
            this.transform.localScale = new Vector2(-scaleX, scaleY);
        }
    }
    */
    #endregion

    #region Fields
    #region FSM
    public EnemyFSM fsm;
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

    #region Getters and Setters
    private float maxTrackDistance;
    public float runDistance
    { get; private set; }
    public float attackDistance
    { get; private set; }
    #endregion
    #endregion

    #region Unity Methods
    private void Start()
    {
        fsm = new EnemyFSM();

        enemyScript = GetComponent<EnemyScript>();
        enemyStats = GetComponent<EnemyStats>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyCombat = GetComponent<EnemyCombat>();

        maxTrackDistance = 15f;
        runDistance = 8f;
        attackDistance = 2.5f;

        targetPos = enemyScript.target.transform;

        fsm.MoveStates(EnemyCommands.Spawned);
    }

    private void FixedUpdate()
    {
        float distanceFromTarget = Vector2.Distance(transform.position, targetPos.position);
        switch (fsm.currentState.thisStateID)
        {
            case EnemyStates.Idle:
                if (distanceFromTarget > maxTrackDistance)
                {
                    // fsm.MoveStates(EnemyCommands.NotInRange);
                }
                else if (distanceFromTarget <= maxTrackDistance)
                {
                    fsm.MoveStates(EnemyCommands.InRange);
                }
                break;
            case EnemyStates.Tracking:
                if (distanceFromTarget >= maxTrackDistance)
                {
                    fsm.MoveStates(EnemyCommands.NotInRange);
                }
                if (distanceFromTarget <= attackDistance)
                {
                    fsm.MoveStates(EnemyCommands.InAttackRange);
                }
                break;
            case EnemyStates.Attacking:
                if (distanceFromTarget > attackDistance)
                {
                    fsm.MoveStates(EnemyCommands.NotInAttackRange);
                }
                break;
            case EnemyStates.Inactive:
                Debug.Log("Dead");
                break;
        }
    }
    #endregion
}
