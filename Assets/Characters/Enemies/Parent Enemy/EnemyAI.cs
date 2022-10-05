using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, ICharacterController
{
    #region Variables
    [SerializeField] private bool canMove = true;
    private bool facingRight = false;

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

    private float distanceFromTarget;
    private float maxTrackDistance = 14f;
    private float runDistance = 8f;
    private float attackDistance = 2.5f;

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
            enemyPosX = this.transform.localScale.x;
            enemyPosY = this.transform.localScale.y;

            // Run action method
            
            Flip(facingRight);
        }
        
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Movement()
    {
        if (canMove == true)
        {
            distanceFromTarget = Vector2.Distance(transform.position, target.position);
            switch (TrackPlayer())
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
                    // Enemy will stand still waiting for player to get in range
                    break;
            }
        }
    }

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
        transform.position = Vector2.MoveTowards(transform.position, target.position, hRunSpeed * Time.deltaTime);
    }

    public void Dodge()
    {
        // Method to make enemy randomly dodge when player is attacking 
    }

    public void Action()
    {
        // Method to make enemy randomly pick up item whenever near one
    }

    public void Flip(bool facingRight)
    {
        if (transform.position.x > target.position.x)
        {
            this.transform.localScale = new Vector2(-1.89751f, enemyPosY);
        }
        else
        {
            this.transform.localScale = new Vector2(1.89751f, enemyPosY);
        }
    }
}
