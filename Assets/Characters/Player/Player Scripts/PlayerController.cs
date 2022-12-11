using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacterController
{
    #region Variables 
    [SerializeField] private bool canMove = true;

    // Variables for player movement
    [SerializeField] private int vSpeed = 2;
    [SerializeField] private int hSpeed = 3;
    [SerializeField] private int vRunSpeed = 4;
    [SerializeField] private int hRunSpeed = 5;
    [SerializeField] private bool isRunning = false;
    private float vMove, hMove;

    [SerializeField] private const int jumpHeight = 3;
    [SerializeField] private bool onGround = true;

    // Variables to allow player to dodge
    [SerializeField] private bool isDodging = false;
    [SerializeField] private float dodgeSpeed = 35;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side = 0;
    private float dodgeTime;  

    // For double tapping key
    private float doubleTapSpeed;
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
        
        // Following sets dodgeTime = to 0.1 as default
        dodgeTime = startDodgeTime; 
    }

    // Update is called once per frame
    void Update()
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

    // Similar to Update but runs depending on device framerate (default 0.02s), better to use with physics
    void FixedUpdate()
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

    // Action button can be worked on at later date when items added
    // Allows for the player to pick up an item on the floor by pressing an input
    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Player did an action");
        }
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
