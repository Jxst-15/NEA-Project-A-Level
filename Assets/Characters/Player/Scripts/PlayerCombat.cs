using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    #region variables
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private bool Attacking
    { get; set; }
    [SerializeField] private bool Blocking
    { get; set; }
    [SerializeField] private float attackRate
    { get; set; }
    [SerializeField] private int attackCount = 0;
    private float nextAttackTime = 0f;
    
    private int combatTime = 3;
    private int comboCount = 0;

    private float parryDelay = 0.5f;
    
    private int throwRate = 5;
    
    private float tapSpeed = 0.5f;
    KeyCode lastKey;
    
    [SerializeField] private string fightStyle = "Iron Fist";
    
    [SerializeField] private int stamDecLAttack = 15;
    [SerializeField] private int stamDecHAttack = 20;
    [SerializeField] private int stamDecWUAttack = 30;
    [SerializeField] private int stamDecThrow = 35;
    [SerializeField] private int stamDecBlock = 10;
    [SerializeField] private int stamDecParry = 20;
    [SerializeField] private int healthDecreaseBlock = 5;
    
    [SerializeField] private GameObject weapon;
    [SerializeField] private bool weaponHeld
    { get; set; }
    
    private Stopwatch timer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Attacking = false;
        Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (weaponHeld == true)
            {
                // WeaponAttack method will be called here
            }
            else
            {
                // Attack method will be called here
            }
        }

        if (Input.GetKey(KeyCode.H))
        {
            // Block method will run here
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            // Parry method will run here
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // SwitchStyle method will run here
        }      
    }

    public int getComboCount()
    {
        return comboCount;
    }
    
    public void resetComboCount()
    {
        comboCount = 0;
    }

    public void comboMeter()
    {
        // Combo meter will be configured here
    }

    public void Attack()
    {
        // Attack feature will be configured here
    }

    public void Throw()
    {
        // Throw feature will be configured here
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
    }

    public void WeaponAttack()
    {
        // Weapon attack feature will be configured here
    }
}
