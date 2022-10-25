using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICharacterCombat
{
    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] private GameObject weapon;

    public GameObject attackBox, blockBox, parryBox;
    
    // LayerMasks help to identify which objects can be hit
    public LayerMask enemyLayer, hittableObject;

    public PlayerAttack playerAttack; 
    public PlayerBlock playerBlock; 

    private float nextWAttackTime = 0f;
    private const float wAttackRate = 2f;

    [SerializeField] private float attackRange;
    [SerializeField] private int attackCount;
    private float nextAttackTime;

    private int comboTime;
    
    // How many attacks player has performed without getting hit (Field)
    private int comboCount;

    // 0.5s, time which is needed for a parry to be valid
    private const float parryDelay = 0.5f;
    
    private float throwRate, nextThrowTime;

    // For determining whether 'H' is being held or pressed
    private const float minHold = 0.2f;
    private float pressedTime;
    private bool keyHeld;
    
    private float tapSpeed;
    KeyCode lastKey;
    
    [SerializeField] private string fightStyle;

    // Variables for decreasing stat factors
    [SerializeField] private int stamDecLAttack, stamDecHAttack, stamDecWUAttack, stamDecThrow, stamIncParry;
    #endregion

    #region Getters and Setters
    [SerializeField] public int stamDecBlock
    { get; set; }
    [SerializeField] public int healthDecBlock
    { get; set; }
    // Property to get and reset the combo count
    public int _comboCount 
    {
        get { return comboCount; }
        set { comboCount = value; }
    }
    public bool canAttack
    { get; set; }
    public bool canDefend
    { get; set; }
    public bool attacking
    { get; set; }
    public bool blocking
    { get; set; }
    public bool throwing
    { get; set; }
    public bool parryable
    { get; set; }
    public float attackRate
    { get; set; }
    public bool weaponHeld
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

        canAttack = true;
        canDefend = true;
        attacking = false;
        blocking = false;
        throwing = false;
        parryable = false;

        attackRange = 1.5f;
        attackCount = 0;
        nextAttackTime = 0f;

        comboTime = 3;

        throwRate = 0.3f;
        nextThrowTime = 0f;

        pressedTime = 0f;
        keyHeld = false;

        fightStyle = "Iron Fist";
        
        attackRate = 2;
        
        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamDecBlock = 10;
        stamIncParry = 20;
        healthDecBlock = 4;

        weaponHeld = false;
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
                WeaponAttack();
            }
            else
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.I))      
            {
                UnityEngine.Debug.Log("Throw Initiated");
                Throw();
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
                blocking = false;
                canAttack = true;
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
        comboCount = 0;
    }

    public void ComboMeter()
    {
        // Combo meter will be configured here
    }

    public void Attack()
    {
        // If the time elapsed since game started is more or equal to nextAttackTime, prevents spam
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown("lAttack"))
            {
                UnityEngine.Debug.Log("Attack Initiated");
                attacking = true;
                parryable = true;

                // Light attack performed
                foreach (Collider2D enemy in playerAttack.GetEnemiesHit())
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
                attacking = true;
                parryable = true;

                // Heavy attack performed
                foreach (Collider2D enemy in playerAttack.GetEnemiesHit())
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
        }
        attacking = false;
        parryable = false;
    }

    // Performs a throw attack on the selected enemy, adds a force to the enemy object and deals damage
    public void Throw()
    {
        if (Time.time >= nextThrowTime)
        {
            // Throw feature will be configured here
            throwing = true;
            attacking = true;
            // Throw attack performed
            // Add force to enemy rigid body 
            UnityEngine.Debug.Log("Throw attack performed");
            attacking = false;

            nextThrowTime = Time.time + 1f / throwRate;
        }
    }

    // Negates all incoming damage from enemies in direction player facing, deals chip damage and stamina to player
    public void Block()
    {
        // Block feature will be configured here
        blocking = true;
        canAttack = false;
        blockBox.SetActive(true);
    }

    // Allows player to parry incoming enemy attacks, player recieves no damage if successful and regains small stamina
    public void Parry()
    {
        // Parry feature will be configured here
        canAttack = false;
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
        if (Time.time >= nextWAttackTime)
        {
            // Weapon attack feature will be configured here
            if (Input.GetButtonDown("lAttack"))
            {
                UnityEngine.Debug.Log("Weapon Attack Initiated");
                attacking = true;
                parryable = true;

                // Light attack performed
                UnityEngine.Debug.Log("Light weapon attack performed");
                nextWAttackTime = Time.time + 1f / wAttackRate;
            }
            else if (Input.GetButtonDown("hAttack"))
            {
                UnityEngine.Debug.Log("Weapon Attack Initiated");
                attacking = true;
                parryable = true;

                // Heavy attack performed
                UnityEngine.Debug.Log("Heavy weapon attack performed");
                nextWAttackTime = Time.time + 1f / wAttackRate;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                UnityEngine.Debug.Log("Weapon Attack Initiated");
                attacking = true;

                // Gets the type of the weapon to determine which attack to perform
                UnityEngine.Debug.Log("Unique weapon attack performed");
                nextWAttackTime = Time.time + 1f / wAttackRate;
            }
        }
        attacking = false;
        parryable = false;
    }
}
