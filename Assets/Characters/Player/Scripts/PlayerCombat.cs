using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] private GameObject weapon;
    
    public GameObject attackBox;
    public GameObject blockBox;
    public GameObject parryBox;
    
    // LayerMasks help to identify which objects can be hit
    public LayerMask enemyLayer;
    public LayerMask hittableObject;

    public PlayerAttack playerAttack; 
    public PlayerBlock playerBlock; 

    private float nextWAttackTime = 0f;
    private const float wAttackRate = 2f;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackCount = 0;
    private float nextAttackTime = 0f;

    private int combatTime = 3;
    
    // How many attacks player has performed without getting hit (Field)
    private int comboCount;

    // 0.5s, time which is needed for a parry to be valid
    private const float parryDelay = 0.5f;
    
    private float throwRate = 0.3f;
    private float nextThrowTime = 0f;

    // For determining whether 'H' is being held or pressed
    private const float minHold = 0.2f;
    private float pressedTime = 0f;
    private bool keyHeld = false;
    
    private float tapSpeed;
    KeyCode lastKey;
    
    [SerializeField] private string fightStyle;

    // Variables for decreasing stat factors
    [SerializeField] private int stamDecLAttack;
    [SerializeField] private int stamDecHAttack;
    [SerializeField] private int stamDecWUAttack;
    [SerializeField] private int stamDecThrow;
    [SerializeField] private int stamIncParry;
    [SerializeField] private int stamDecBlock
    { get; set; }
    [SerializeField] private int healthDecBlock
    { get; set; }
    #endregion

    #region Getters and Setters

    // Property to get and reset the combo count
    private int _comboCount 
    {
        get { return comboCount; }
        set { comboCount = value; }
    }
    private bool canAttack
    { get; set; }
    private bool canDefend
    { get; set; }
    private bool attacking
    { get; set; }
    private bool blocking
    { get; set; }
    private bool throwing
    { get; set; }
    private float attackRate
    { get; set; }
    private bool weaponHeld
    { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Gets the PlayerAttack script from the attackBox GameObject
        playerAttack = attackBox.GetComponent<PlayerAttack>(); 
        
        // Gets the PlayerBlock script from the blockBox GameObject
        playerBlock = blockBox.GetComponent<PlayerBlock>(); 

        blockBox.SetActive(false);
        parryBox.SetActive(false);

        this.canAttack = true;
        this.canDefend = true;
        this.blocking = false;

        fightStyle = "Iron Fist";
        
        attackRate = 2;
        
        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamDecBlock = 10;
        stamIncParry = 20;
        healthDecBlock = 4;
    }

    // Update is called once per frame
    void Update()
    {
        ComboMeter();

        // Checks if player is holding a weapon, if yes then set weaponHeld to true
        if (weapon != null && weapon.tag == "Weapons") 
        {
            weaponHeld = true;
        }

        // If player is able to attack
        if (canAttack == true) 
        {
            if (weaponHeld == true)
            {
                if (Time.time >= nextWAttackTime)
                {
                    WeaponAttack();
                }
            }
            else
            {
                // If the time elapsed since game started is more or equal to nextAttackTime, prevents spam
                if (Time.time >= nextAttackTime)
                {
                    Attack();
                }
            }
            if (Input.GetKeyDown(KeyCode.I))      
            {
                if (Time.time >= nextThrowTime)
                {
                    UnityEngine.Debug.Log("Throw Initiated");
                    Throw();
                }
            }
        }

        // If player is able to defend 
        if (canDefend == true)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                // Equal to time elapsed
                pressedTime = Time.time;               
                keyHeld = false;
            }
            // Once key has been released, knows that key has been pressed not held
            else if (Input.GetKeyUp(KeyCode.H)) 
            {
                if (keyHeld == false)
                {
                    UnityEngine.Debug.Log("Parry initiated");
                    Parry();

                }
                keyHeld = false;
            }
            if (Input.GetKey(KeyCode.H))
            {
                // If time elapsed - pressedTime > minHold (0.2s) then knows key has been held
                if (Time.time - pressedTime > minHold)
                {
                    Block();
                    keyHeld = true;
                }
            }
            // Once key has been released then player is no longer blocking and can attack 
            else
            {
                parryBox.SetActive(false);
                blockBox.SetActive(false);
                this.blocking = false;
                this.canAttack = true;
            }
        }

        // If LeftShift is held then will call SwitchStyle method
        if (Input.GetKey(KeyCode.LeftShift))
        {
            SwitchStyle();
        }
        else
        {
            // Sets game time back to normal
            Time.timeScale = 1f;
        }
    }   
 
    public void ResetComboCount()
    {
        this.comboCount = 0;
    }

    public void ComboMeter()
    {
        // Combo meter will be configured here
    }

    public void Attack()
    {   
        if (Input.GetButtonDown("lAttack"))
        {
            UnityEngine.Debug.Log("Attack Initiated");
            this.attacking = true;
            
            // Light attack performed
            foreach(Collider2D enemy in playerAttack.GetEnemiesHit())
            {
                UnityEngine.Debug.Log("Enemy Hit! (L)");
                enemy.GetComponent<EnemyStats>().takeDamage(50);
                comboCount++;
            }
            UnityEngine.Debug.Log("Light attack performed");
            // Decrease current stamina by stamDecLAttack
            attackCount++;
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else if (Input.GetButtonDown("hAttack"))
        {
            UnityEngine.Debug.Log("Attack Initiated");
            this.attacking = true;
            
            // Heavy attack performed
            foreach(Collider2D enemy in playerAttack.GetEnemiesHit())
            {
                UnityEngine.Debug.Log("Enemy Hit! (H)");
                enemy.GetComponent<EnemyStats>().takeDamage(75);
                comboCount++;
            }
            UnityEngine.Debug.Log("Heavy attack performed");
            // Decrease current stamina by stamDecHAttack
            attackCount++;
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
        this.attacking = false;
    }

    // Performs a throw attack on the selected enemy, adds a force to the enemy object and deals damage
    public void Throw()
    {
        // Throw feature will be configured here
        this.throwing = true;
        this.attacking = true;
        // Throw attack performed
        // Add force to enemy rigid body 
        UnityEngine.Debug.Log("Throw attack performed");
        this.attacking = false;
        
        nextThrowTime = Time.time + 1f / throwRate;
    }

    // Negates all incoming damage from enemies in direction player facing, deals chip damage and stamina to player
    public void Block()
    {
        // Block feature will be configured here
        this.blocking = true;
        this.canAttack = false;
        blockBox.SetActive(true);
    }

    // Allows player to parry incoming enemy attacks, player recieves no damage if successful and regains small stamina
    public void Parry()
    {
        // Parry feature will be configured here
        this.canAttack = false;
        parryBox.SetActive(true);
    }

    public void SwitchStyle()
    {
        // Slows down game time by half
        Time.timeScale = 0.5f;
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            fightStyle = "Iron Fist";
            
            attackRate = 2;
            
            stamDecLAttack = 15;
            stamDecHAttack = 20;
            stamDecThrow = 35;
            healthDecBlock = 4;
            UnityEngine.Debug.Log("Current Style Is: " + fightStyle);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            fightStyle = "Boulder Style";

            attackRate = 1f;

            stamDecLAttack = 25;
            stamDecHAttack = 30;
            stamDecThrow = 45;
            healthDecBlock = 2;
            UnityEngine.Debug.Log("Current Style Is: " + fightStyle);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            fightStyle = "Grass Style";

            attackRate = 3f;

            stamDecLAttack = 5;
            stamDecHAttack = 10;
            stamDecThrow = 25;
            healthDecBlock = 6;
            UnityEngine.Debug.Log("Current Style Is: " + fightStyle);
        }
    }

    public void WeaponAttack()
    {
        // Weapon attack feature will be configured here
        if (Input.GetButtonDown("lAttack"))
        {
            UnityEngine.Debug.Log("Weapon Attack Initiated");
            this.attacking = true;
            
            // Light attack performed
            UnityEngine.Debug.Log("Light weapon attack performed");
            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
        else if (Input.GetButtonDown("hAttack"))
        {
            UnityEngine.Debug.Log("Weapon Attack Initiated");
            this.attacking = true;
            
            // Heavy attack performed
            UnityEngine.Debug.Log("Heavy weapon attack performed");
            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            UnityEngine.Debug.Log("Weapon Attack Initiated");
            this.attacking = true;
            
            // Gets the type of the weapon to determine which attack to perform
            UnityEngine.Debug.Log("Unique weapon attack performed");
            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
    }
}
