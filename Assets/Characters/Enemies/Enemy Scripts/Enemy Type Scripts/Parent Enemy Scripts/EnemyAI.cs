using UnityEngine;

public class EnemyAI : MonoBehaviour, ICharacterController
{
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
    private float enemyPosX, enemyPosY;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        enemyStats = GetComponent<EnemyStats>();
        enemyScript = GetComponent<EnemyScript>();
        SetVariables();
        // Finds the target by getting the target variable from the EnemyScript parent class and gets the transform component 
        targetPos = enemyScript.target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused == false|| canMove == false)
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

    }

    public void EAIUpdate()
    {
        if (canMove == true)
        {
            enemyPosX = this.transform.localScale.x;
            enemyPosY = this.transform.localScale.y;

            // Run action method

            // For flipping the sprite
            Flip(notNeeded, enemyPosX, enemyPosY);
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
            float distanceFromTarget = Vector2.Distance(transform.position, targetPos.position);
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
    public void Flip(bool notNeeded, float posX, float posY)
    {
        if (transform.position.x > targetPos.position.x)
        {
            this.transform.localScale = new Vector2(-1.89751f, posY);
        }
        else
        {
            this.transform.localScale = new Vector2(1.89751f, posY);
        }
    }
}
