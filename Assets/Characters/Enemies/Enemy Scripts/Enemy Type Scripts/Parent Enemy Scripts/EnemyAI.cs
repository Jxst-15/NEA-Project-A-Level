using UnityEngine;

public class EnemyAI : EnemyScript, ICharacterController
{
    #region Variables
    private const bool notNeeded = false;
    [SerializeField] private bool canMove = true;

    public Transform targetPos;
    //[SerializeField] private int vSpeed = 2;
    //[SerializeField] private int hSpeed = 3;
    //[SerializeField] private int vRunSpeed = 4;
    //[SerializeField] private int hRunSpeed = 5;
    [SerializeField] private bool isRunning = false;
    private float enemyPosX, enemyPosY;

    [SerializeField] private bool isDodging = false;
    [SerializeField] private float dodgeSpeed = 35;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side = 0;
    private float dodgeTime;

    private float distanceFromTarget;
    private float maxTrackDistance = 14f;
    private float runDistance = 8f;
    private float attackDistance = 2.5f;

    private Rigidbody2D rb;
    #endregion

    #region Getters and Setters
    public int vSpeed
    { get; set; }
    public int hSpeed
    { get; set; }
    public int vRunSpeed
    { get; set; }
    public int hRunSpeed
    { get; set; }
    [SerializeField] public bool inRange
    { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Finds the target by getting the target variable from the EnemyScript parent class and gets the transform component 
        targetPos = target.transform;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (canMove == true)
    //    {
    //        enemyPosX = this.transform.localScale.x;
    //        enemyPosY = this.transform.localScale.y;

    //        // Run action method

    //        // For flipping the sprite
    //        Flip(notNeeded, enemyPosX, enemyPosY);
    //    }

    //}

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

    private void FixedUpdate()
    {
        Movement();
    }

    // Actually moving this object towards the target
    public virtual void Movement()
    {
        if (canMove == true)
        {
            distanceFromTarget = Vector2.Distance(transform.position, targetPos.position);
            switch (TrackPlayer())
            {
                case 0:
                    // Make enemy run towards player
                    inRange = false;
                    Run();
                    break;
                case 1:
                    // Make enemy walk towards player
                    inRange = false;
                    transform.position = Vector2.MoveTowards(transform.position, targetPos.position, hSpeed * Time.deltaTime);
                    break;
                case 2:
                    // Enemy in attack range, make enemy attack and stop moving
                    inRange = true;
                    // Also random chance of dodging whenever player attacks
                    break;
                case -1:
                    // Enemy will stand still waiting for player to get in range
                    inRange = false;
                    break;
            }
        }
    }

    // Determining the distance between this object and the player
    private int TrackPlayer()
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
