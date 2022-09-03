using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region variables
    public LayerMask enemyLayer;
    public LayerMask hittableObject;
    public GameObject attackBox;

    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private bool canAttack
    { get; set; }
    [SerializeField] private bool attacking
    { get; set; }
    [SerializeField] private bool blocking
    { get; set; }
    [SerializeField] private bool throwing
    { get; set; }
    [SerializeField] private float attackRate
    { get; set; }
    [SerializeField] private int attackCount = 0;
    [SerializeField] private float nextAttackTime = 0f;
    
    private int combatTime = 3;
    private int comboCount = 0;

    private const float parryDelay = 0.5f;
    
    private const float throwRate = 0.5f;
    private float nextThrowTime = 0f;
    
    private float tapSpeed;
    KeyCode lastKey;
    
    [SerializeField] private string fightStyle;
    
    [SerializeField] private int stamDecLAttack;
    [SerializeField] private int stamDecHAttack;
    [SerializeField] private int stamDecWUAttack;
    [SerializeField] private int stamDecThrow;
    [SerializeField] private int stamDecBlock;
    [SerializeField] private int stamIncParry;
    [SerializeField] private int healthDecBlock;
    
    [SerializeField] private GameObject weapon;
    [SerializeField] private bool weaponHeld
    { get; set; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        attackBox.SetActive(false);
        this.canAttack = true;
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
        
        if (weapon != null)
        {
            weaponHeld = true;
        }

        if (canAttack == true)
        {
            if (weaponHeld == true)
            {
                // WeaponAttack method will be called here
                UnityEngine.Debug.Log("Weapon Attack Initiated");
                WeaponAttack();
            }
            else
            {
                // Attack method will be called here
                // UnityEngine.Debug.Log("Attack Initiated");
                Attack();
            }
        }
      
        if (Input.GetKeyDown(KeyCode.I))      
        {
            if (Time.time >= nextThrowTime && canAttack == true)
            {
                // Throw method will be called here
                UnityEngine.Debug.Log("Throw Initiated");
                Throw();
            }
        }

        if (Input.GetKey(KeyCode.H))
        {
            // Block method will run here
            UnityEngine.Debug.Log("Block Initiated");
            Block();
        }
        else
        {
            this.blocking = false;
            this.canAttack = true;
        }
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Parry method will run here
            UnityEngine.Debug.Log("Parry initiated");
            Parry();
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // SwitchStyle method will run here
            UnityEngine.Debug.Log("Switch Style Initiated");
            SwitchStyle();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public int GetComboCount()
    {
        return comboCount;
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
        // Attack feature will be configured here
        if (Input.GetButtonDown("lAttack"))
        {
            if (Time.time >= nextAttackTime)
            {
                this.attacking = true;
                attackBox.SetActive(true);
                // Light attack performed
                UnityEngine.Debug.Log("Light attack performed");
            nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else if (Input.GetButtonDown("hAttack"))
        {
            if (Time.time >= nextAttackTime)
            {
                this.attacking = true;
                attackBox.SetActive(true);
                // Heavy attack performed
                UnityEngine.Debug.Log("Heavy attack performed");
            nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        attackBox.SetActive(false);
        this.attacking = false;
        // nextAttackTime = Time.time + 1f / attackRate;
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
    }

    public void Parry()
    {
        // Parry feature will be configured here
    }

    public void SwitchStyle()
    {
        // Switch style feature will be configured here
        Time.timeScale = 0.5f;
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            fightStyle = "Iron Fist";
            
            attackRate = 2;
            
            stamDecLAttack = 15;
            stamDecHAttack = 20;
            stamDecThrow = 35;
            healthDecBlock = 4;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            fightStyle = "Boulder Style";

            attackRate = 1;

            stamDecLAttack = 25;
            stamDecHAttack = 30;
            stamDecThrow = 45;
            healthDecBlock = 2;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            fightStyle = "Grass Style";

            attackRate = 3;

            stamDecLAttack = 5;
            stamDecHAttack = 10;
            stamDecThrow = 25;
            healthDecBlock = 6;
        }

        UnityEngine.Debug.Log("Current Style Is: " + fightStyle);
    }

    public void WeaponAttack()
    {
        // Weapon attack feature will be configured here
        if (Input.GetButtonDown("lAttack"))
        {
            this.attacking = true;
            // Light attack performed
            UnityEngine.Debug.Log("Light weapon attack performed");
        }
        else if (Input.GetButtonDown("hAttack"))
        {
            this.attacking = true;
            // Heavy attack performed
            UnityEngine.Debug.Log("Heavy weapon attack performed");
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            // Gets the type of the weapon to determine which attack to perform
        }
    }
}
