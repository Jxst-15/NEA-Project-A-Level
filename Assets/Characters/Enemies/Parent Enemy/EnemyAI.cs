using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : EnemyFSM
{
    #region Variables
    [SerializeField] private bool canMove = true;

    public Transform target;
    [SerializeField] private int vSpeed = 2;
    [SerializeField] private int hSpeed = 3;
    [SerializeField] private int vRunSpeed = 4;
    [SerializeField] private int hRunSpeed = 5;
    [SerializeField] private bool isRunning = false;
    private float enemyPosX, enemyPosY;

    [SerializeField] private bool isDodging = false;
    [SerializeField] private float dodgeSpeed = 35;
    [SerializeField] private float startDodgeTime;
    [SerializeField] private int side = 0;
    private float dodgeTime;

    private float runDistance = 8f;
    private float attackDistance = 3f;

    private Rigidbody2D rb;
    #endregion

    // Start is called before the first frame update
    void Start()
    {      
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove == true)
        {
            enemyPosX = transform.localScale.x;
            enemyPosY = transform.localScale.y;

            // Run action method
            
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        if (canMove == true)
        {
            switch(TrackPlayer())
            {
                case 0:
                    // Make enemy run towards player
                    Run();
                    break;
                case 1:
                    // Make enemy walk towards player
                    transform.position = Vector2.MoveTowards(transform.position, target.position, hSpeed * Time.deltaTime);
                    break;
                case 2:
                    // Enemy in attack range, make enemy attack and stop moving
                    // Also random chance of dodging whenever player attacks
                    break;
                case -1:
                    // Debug.Log("Enemy out of range");
                    // Enemy will stand still waiting for player to get in range
                    break;
            }
        }
        
    }

    private int TrackPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) >= runDistance)
        {
            return 0;
        }
        else if (Vector2.Distance(transform.position, target.position) < runDistance && Vector2.Distance(transform.position, target.position) > attackDistance)
        {
            return 1;
        }
        else if (Vector2.Distance(transform.position, target.position)  <= attackDistance)
        {
            return 2;
        }
        
        return -1;
    }

    // Method to make enemy run towards player
    private void Run()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, hRunSpeed * Time.deltaTime);
    }

    private void Dodge()
    {
        // Method to make enemy randomly dodge when player is attacking 
    }

    private void Action()
    {
        // Method to make enemy randomly pick up item whenever near one
    }

    public void Flip()
    {
        if (transform.position.x > target.position.x)
        {
            this.transform.localScale = new Vector2(enemyPosX, enemyPosY);
        }
        else if (transform.position.x <= target.position.x)
        {
            this.transform.localScale = new Vector2(-enemyPosX, enemyPosY);
        }
    }
}