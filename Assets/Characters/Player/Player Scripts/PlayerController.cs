using UnityEngine;

public class PlayerController : CharMovement, ICharacterController
{
    #region GameObjects
    public GameObject actionBox;
    #endregion

    #region Script References
    public PlayerAction playerAction;
    #endregion

    #region Variables 
    // Variables for player movement
    [SerializeField] private int vSpeed;
    [SerializeField] private int hSpeed;
    [SerializeField] private int vRunSpeed;
    [SerializeField] private int hRunSpeed;
    [SerializeField] private bool isRunning;
    private float vMove, hMove;

    [SerializeField] private const int jumpHeight = 3;
    [SerializeField] private bool onGround;

    // Variables to allow player to dodge
    [SerializeField] private bool isDodging;
    [SerializeField] private float dodgeSpeed;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side;
    private float dodgeTime;  

    // For double tapping key
    private float doubleTapSpeed;
    KeyCode lastKey;

    // For flipping player
    private bool facingRight;
    private float playerPosX, playerPosY;
    
    private Rigidbody2D rb;
    #endregion

    #region Getters and Setters
    public bool canMove
    { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetVariables();

        playerAction = actionBox.GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            // If player able to move
            if (canMove == true)
            {
                // Gets raw number value of axis
                vMove = Input.GetAxisRaw("Vertical");
                hMove = Input.GetAxisRaw("Horizontal");
                DoubleTappingMovement();
                Flip(facingRight, playerPosX, playerPosY);
            }
            Action();
        }
    }

    // Similar to Update but runs depending on device framerate (default 0.02s), better to use with physics
    void FixedUpdate()
    {
        if (PauseMenu.isPaused == false)
        {
            isRunning = false;
            isDodging = false;
            if (canMove == true)
            {
                Jump();

                // If player not currently dodging or running
                if (isDodging == false && isRunning == false)
                {
                    // Player can move, adds velocity to rigidbody (value x axis * hSpeed, value y axis * vSpeed)
                    Movement();
                }

                // If player wants to sprint
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    isRunning = true;
                    Run();
                }

                Dodge();
            }
        }
    }

    private void SetVariables()
    {
        playerPosX = transform.localScale.x;
        playerPosY = transform.localScale.y;

        vSpeed = 2;
        hSpeed = 3;
        vRunSpeed = 4;
        hRunSpeed = 5;

        canMove = true;
        isRunning = false;

        // Following sets dodgeTime = to 0.1 as default
        dodgeTime = startDodgeTime;
        isDodging = false;
        dodgeSpeed = 35;
        side = 0;

        facingRight = true;
        
        onGround = true;

    }

    // WIP
    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            onGround = false;
            Debug.Log("Player jumped!");
            // rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
            // onGround = true;
        }
    }

    public void DoubleTappingMovement()
    {
        if (hMove > 0)
        {
            facingRight = true;
            if (Input.GetKeyDown(KeyCode.D))
            {
                // If the defined double tap speed > time elapsed & lastkey pressed = 'D'
                if (doubleTapSpeed > Time.time && lastKey == KeyCode.D)
                {
                    side = 2;
                    // doubleTapped = true;
                    isDodging = true;
                }
                else
                {
                    // Double tap speed updated to time elapsed + 0.3 seconds
                    doubleTapSpeed = Time.time + 0.3f;
                }
                lastKey = KeyCode.D;
            }
        }
        // If player moving left as x axis < 0 (-1) means player facing left
        else if (hMove < 0)
        {
            facingRight = false;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (doubleTapSpeed > Time.time && lastKey == KeyCode.A)
                {
                    // Indicates left side
                    side = 1;
                    // doubleTapped = true; // Key has been double tapped
                    isDodging = true;
                }
                else
                {
                    doubleTapSpeed = Time.time + 0.3f;
                }
                // Last key pressed is set to A
                lastKey = KeyCode.A;
            }
        }
    }

    public void Movement()
    {
        rb.velocity = new Vector2(hMove * hSpeed, vMove * vSpeed);
    }

    public void Run()
    {
        // Similar to normal movement but speed values replaced with run speed values
        rb.velocity = new Vector2(hMove * hRunSpeed, vMove * vRunSpeed);
    }

    public void Dodge()
    {
        if (side != 0)
        {
            if (dodgeTime <= 0)
            {
                side = 0;
                dodgeTime = startDodgeTime;
                rb.velocity = Vector2.zero;
            }
            else
            {
                dodgeTime -= Time.deltaTime;
                if (side == 1)
                {
                    rb.velocity = Vector2.left * dodgeSpeed;
                }
                else if (side == 2)
                {
                    rb.velocity = Vector2.right * dodgeSpeed;
                }
            }
        }
    }

    // Action button can be worked on at later date when items added
    // Allows for the player interact with various objects in the stage e.g. weapons
    public void Action()
    {
        playerAction.Action();
    }

    // Takes in bool flag to determine which direction player facing and flips player accordingly
    public void Flip(bool facingRight, float posX, float posY) 
    {
        if (facingRight == true) {
            this.transform.localScale = new Vector2(posX, posY);
        }
        else 
        {
            // Flips player on the x axis so it is negative so player faces left
            this.transform.localScale = new Vector2(-posX, posY);
        }
    }
}
