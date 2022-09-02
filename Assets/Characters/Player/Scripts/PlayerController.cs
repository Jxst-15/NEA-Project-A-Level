using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables 
    [SerializeField] private bool canMove = true;

    // Variables for player movement
    [SerializeField] private int vSpeed = 2;
    [SerializeField] private int hSpeed = 4;
    [SerializeField] private int vRunSpeed = 5;
    [SerializeField] private int hRunSpeed = 7;
    [SerializeField] private bool isRunning = false;
    private float vMove, hMove;

    [SerializeField] private int jumpHeight = 3;
    [SerializeField] private bool onGround = true;

    // Variables to allow player to dodge
    [SerializeField] private bool isDodging = false;
    [SerializeField] private float dodgeSpeed = 35;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side = 0;
    private float dodgeTime;

    // For double tapping key
    private float tapSpeed;
    KeyCode lastKey;

    // For flipping player
    private bool facingRight = true;
    private float playerPosX, playerPosY;
    
    private Rigidbody2D rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPosX = transform.localScale.x;
        playerPosY = transform.localScale.y;
        dodgeTime = startDodgeTime; // Sets dodgeTime = to 0.1 as default
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true) // If player able to move
        {   
            // Gets raw number value of axis
            vMove = Input.GetAxisRaw("Vertical"); 
            hMove = Input.GetAxisRaw("Horizontal");       
            if (hMove > 0)
            {
                facingRight = true;
                flip(facingRight); // Flip method runs
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (tapSpeed > Time.time && lastKey == KeyCode.D) // If the defined double tap speed > time elapsed & lastkey pressed = 'D'
                    {
                        side = 2;
                        // doubleTapped = true;
                        isDodging = true;
                    }
                    else
                    {
                        tapSpeed = Time.time + 0.3f; // Double tap speed updated to time elapsed + 0.3 seconds
                    }
                    lastKey = KeyCode.D;
                }
            }
            else if (hMove < 0) // If player moving left as x axis < 0 (-1) means player facing left left
            {
                facingRight = false;
                flip(facingRight);
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (tapSpeed > Time.time && lastKey == KeyCode.A)
                    {
                        side = 1; // Indicates left side
                        // doubleTapped = true; // Key has been double tapped
                        isDodging = true;
                    }
                    else
                    {
                        tapSpeed = Time.time + 0.3f;
                    }
                    lastKey = KeyCode.A; // Last key pressed is set to A
                }
            }
        }
        action();
    }


    // Similar to Update but runs depending on device framerate (default 0.02s), better to use with physics
    void FixedUpdate()
    {
        isRunning = false;
        isDodging = false;
        if (canMove == true)
        {
            jump(); // Jump method called

            if (isDodging == false && isRunning == false) // If player not currently dodging or running
            {
                rb.velocity = new Vector2(hMove * hSpeed, vMove * vSpeed); // Player can move, adds velocity to rigidbody
                                                                           // (val x axis * hSpeed, val y axis * vSpeed)
            }
            
            if (Input.GetKey(KeyCode.LeftControl)) // If player wants to sprint
            {
                isRunning = true;
                run();
            }

            dodge();
        }
    }

    public void jump()
    { 
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            onGround = false;
            Debug.Log("Player jumped!");
            // rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
        onGround = true;
    }

    public void run()
    {
        rb.velocity = new Vector2(hMove * hRunSpeed, vMove * vRunSpeed); // Similar to normal movement but speed values replaced with run speed values
    }

    public void dodge()
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
                    // Debug.Log("Dodge initiated (left)");
                    rb.velocity = Vector2.left * dodgeSpeed;
                }
                else if (side == 2)
                {
                    // Debug.Log("Dodge initiated (right)");
                    rb.velocity = Vector2.right * dodgeSpeed;
                }
            }
        }
    }

    public void action() // Action button can be worked on at later date when items added
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Player did an action");
        }
        // Action Button WIP
    }

    public void flip(bool facingRight) // Takes in bool flag to determine which direction player facing
    {
        if (facingRight == true) {
            this.transform.localScale = new Vector2(playerPosX, playerPosY);
        }
        else {
            this.transform.localScale = new Vector2(-playerPosX, playerPosY); // Flips player on the x axis so it is negative
        }
    }
}
