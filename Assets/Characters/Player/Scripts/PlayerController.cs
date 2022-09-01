using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region variables 
    [SerializeField] private bool canMove = true;

    [SerializeField] private int vSpeed = 2;
    [SerializeField] private int hSpeed = 4;
    [SerializeField] private int runSpeed = 6;

    [SerializeField] private int jumpHeight = 3;
    [SerializeField] private bool onGround = true;

    [SerializeField] private int dodgeSpeed = 45;
    private float startDodgeTime;
    private float dodgeTime;

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
        dodgeTime = startDodgeTime;
        playerPosX = transform.localScale.x;
        playerPosY = transform.localScale.y;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            // jump();
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                run();
            }
            
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
        dodge();
    }


    void FixedUpdate()
    {
        if (canMove == true)
        {
            rb.velocity = new Vector2(hMove * hSpeed, vMove * vSpeed);
        }
    }

    //public void jump()
    //{
    //    if (Input.GetButtonDown("Jump") && onGround == true)
    //    {
    //        this.rb.AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    //    }
    //}

    public void run()
    {
        if (Input.GetButtonDown("Horizontal")) {
            switch(Input.GetAxisRaw("Horizontal"))
            {
                case > 0:
                    // Move player by runSpeed to the right
                    break;
                case < 0:
                    // Move player by runSpeed to the left
                    break;
            }
        }
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
    }

    public void action()
    {
        // Action Button WIP
    }
}
