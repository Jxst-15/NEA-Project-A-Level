using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject weapon; // A weapon that has been picked up
    public GameObject attackBox;
    public GameObject blockBox;
    // LayerMasks help to identify which objects can be hit
    public LayerMask enemyLayer;
    public LayerMask hittableObject;

    public PlayerAttack playerAttack;

    private float nextWAttackTime = 0f;
    private const float wAttackRate = 2f;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackCount = 0;
    private float nextAttackTime = 0f;

    private int combatTime = 3;
    private int comboCount; // How many attacks player has performed without getting hit (Field)

    private const float parryDelay = 0.5f; // 0.5s, time which is needed for a parry to be valid
    
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
    [SerializeField] private int stamDecBlock;
    [SerializeField] private int stamIncParry;
    [SerializeField] private int healthDecBlock;

    #endregion

    #region Getters and 
    private int _comboCount // Property to get and reset the combo count
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
        playerAttack = attackBox.GetComponent<PlayerAttack>(); // Gets the PlayerAttack script from the attackBox GameObject

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
        
        if (weapon != null && weapon.tag == "Weapons") // Checks if player is holding a weapon, if yes then set weaponHeld to true
        {
            weaponHeld = true;
        }

        if (canAttack == true) // If player is able to attack
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
                if (Time.time >= nextAttackTime) // If the time elapsed since game started is more or equal to nextAttackTime, prevents spam
                {
                    Attack();
                }
            }
            if (Input.GetKeyDown(KeyCode.I))      
            {
                if (Time.time >= nextThrowTime)
                {
                    UnityEngine.Debug.Log("Throw Initiated");
                    Throw(); // Throw method called
                }
            }
        }

        if (canDefend == true) // If player is able to defend
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                pressedTime = Time.time; // Equal to time elapsed
                keyHeld = false;
            }
            else if (Input.GetKeyUp(KeyCode.H)) // Once key has been released, knows that key has been pressed not held
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
                if (Time.time - pressedTime > minHold) // If time elapsed - pressedTime > minHold (0.2s) then knows key has been held
                {
                    UnityEngine.Debug.Log("Block Initiated");
                    Block(); // Block method called
                    keyHeld = true;
                }
            }
            else // Once key has been released then player is no longer blocking and can attack 
            {
                blockBox.SetActive(false);
                this.blocking = false;
                this.canAttack = true;
            }
        }
               
        if (Input.GetKey(KeyCode.LeftShift)) // If LeftShift is held then will call SwitchStyle method
        {
            SwitchStyle();
        }
        else
        {
            Time.timeScale = 1f; // Sets game time back to normal
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
        // Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackBoxTransform.position, attackRange, enemyLayer);
        
        if (Input.GetButtonDown("lAttack"))
        {
            UnityEngine.Debug.Log("Attack Initiated");
            this.attacking = true;
            // Light attack performed
            foreach(Collider2D enemy in playerAttack.GetEnemiesHit())
            {
                // Deal light damage to enemy
                UnityEngine.Debug.Log("Enemy Hit! (L)");
            }
            UnityEngine.Debug.Log("Light attack performed");
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
        else if (Input.GetButtonDown("hAttack"))
        {
            UnityEngine.Debug.Log("Attack Initiated");
            this.attacking = true;
            // Heavy attack performed
            foreach(Collider2D enemy in playerAttack.GetEnemiesHit())
            {
                // Deal heavy damage to enemy
                UnityEngine.Debug.Log("Enemy Hit! (H)");
            }
            UnityEngine.Debug.Log("Heavy attack performed");
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
        attacking = false;
    }

    public void Throw()
    {
        // Throw feature will be configured here
        this.throwing = true;
        this.attacking = true;
        // Throw attack performed
        UnityEngine.Debug.Log("Throw attack performed");
        this.attacking = false;
        
        nextThrowTime = Time.time + 1f / throwRate;
    }

    public void Block()
    {
        // Block feature will be configured here
        this.blocking = true;
        this.canAttack = false;
        blockBox.SetActive(true);
    }

    public void Parry()
    {
        // Parry feature will be configured here
    }

    public void SwitchStyle()
    {
        Time.timeScale = 0.5f; // Slows down game time by half
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
