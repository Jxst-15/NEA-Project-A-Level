using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables 
    [SerializeField] private bool canMove = true;

    [SerializeField] private int vSpeed = 2;
    [SerializeField] private int hSpeed = 4;
    [SerializeField] private int vRunSpeed = 5;
    [SerializeField] private int hRunSpeed = 7;
    [SerializeField] private bool isRunning = false;

    [SerializeField] private int jumpHeight = 3;
    [SerializeField] private bool onGround = true;

    [SerializeField] private bool isDodging = false;
    [SerializeField] private float dodgeSpeed = 8;
    [SerializeField] private float startDodgeTime;
    private float dodgeTime;
    private float dodgeCooldown = 2f;
    private float nextDodge = 0f;

    private float tapSpeed;
    KeyCode lastKey;

    [SerializeField] private int side = 0;

    private float playerPosX, playerPosY;
    private Rigidbody2D rb;
    private float vMove, hMove;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerPosX = transform.localScale.x;
        playerPosY = transform.localScale.y;
        dodgeTime = startDodgeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            jump();
            
            vMove = Input.GetAxisRaw("Vertical");
            hMove = Input.GetAxisRaw("Horizontal");       
            if (hMove > 0)
            {
                this.transform.localScale = new Vector2(playerPosX, playerPosY);
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (tapSpeed > Time.time && lastKey == KeyCode.D)
                    {
                        side = 2;
                    }
                    else
                    {
                        tapSpeed = Time.time + 0.3f;
                    }
                    lastKey = KeyCode.D;
                }
            }
            else if (hMove < 0)
            {
                this.transform.localScale = new Vector2(-playerPosX, playerPosY);
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (tapSpeed > Time.time && lastKey == KeyCode.A)
                    {
                        side = 1;
                    }
                    else
                    {
                        tapSpeed = Time.time + 0.3f;
                    }
                    lastKey = KeyCode.A;
                }
            }
        }
        
        action();
        if (Time.time > nextDodge) // Need to work on dodge, work on functionality + cooldown upon use
        {
            dodge();
        }
    }


    void FixedUpdate()
    {
        isRunning = false;
        if (canMove == true)
        {
            if (isDodging == false && isRunning == false)
            {
                rb.velocity = new Vector2(hMove * hSpeed, vMove * vSpeed);
            }
            
            if (Input.GetKey(KeyCode.LeftControl))
            {
                isRunning = true;
                run();
            }
        }
    }

    public void jump()
    { 
        if (Input.GetButtonDown("Jump") && onGround == true)
        {
            Debug.Log("Player jumped!");
            // rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
        }
    }

    public void run()
    {
        rb.velocity = new Vector2(hMove * hRunSpeed, vMove * vRunSpeed);
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
                    rb.velocity = Vector2.left * dodgeSpeed;
                }
                else if (side == 2)
                {
                    rb.velocity = Vector2.right * dodgeSpeed;
                }
            }
        }
        nextDodge = Time.time + dodgeCooldown;
    }

    public void action() // Action button can be worked on at later date when items added
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Player did an action");
        }
        // Action Button WIP
    }
}
