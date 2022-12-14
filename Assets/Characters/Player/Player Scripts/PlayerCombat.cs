using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICharacterCombat
{
    #region Script References
    public PlayerStats stats;
    public PlayerStyleSwitch playerStyleSwitch;
    
    public PlayerAttack playerAttack; 
    public PlayerBlock playerBlock; 
    #endregion

    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] public GameObject weapon;

    public GameObject attackBox, blockBox, parryBox;
    
    // LayerMasks help to identify which objects can be hit
    public LayerMask enemyLayer, canHit;

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

    private bool lightAtk;
    #endregion

    #region Getters and Setters
    // Variables for decreasing/increasing stat factors
    public int stamDecLAttack
    { get; set; }
    public int stamDecHAttack
    { get; set; }
    public int stamDecWUAttack
    { get; set; }
    public int stamDecThrow
    { get; set; }
    public int stamIncParry
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

    // Used for getting the required scripts
    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerStyleSwitch = GetComponent<PlayerStyleSwitch>();
        
        // Gets the PlayerAttack script from the attackBox GameObject
        playerAttack = attackBox.GetComponent<PlayerAttack>(); 
        
        // Gets the PlayerBlock script from the blockBox GameObject
        playerBlock = blockBox.GetComponent<PlayerBlock>(); 
        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused == false)
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
                if (Input.GetButtonDown("lAttack") || Input.GetButtonDown("hAttack"))
                {
                    if (Input.GetButtonDown("lAttack"))
                    {
                        lightAtk = true;
                    }
                    else if (Input.GetButtonDown("hAttack"))
                    {
                        lightAtk = false;
                    }

                    if (weapon == null)
                    {
                        Attack();
                    }
                    else if (weapon != null && weapon.tag == "Weapons")
                    {
                        WeaponAttack();
                    }
                }
                if (Input.GetKeyDown(KeyCode.I))
                {
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
                canAttack = true;
                canDefend = true;
            }

            // TEMPORARY
            if (Input.GetKeyDown(KeyCode.B))
            {
                DropWeapon();
            }
        }
    }   

    private void SetVariables()
    {
        blockBox.SetActive(false);
        parryBox.SetActive(false);

        enemyLayer = LayerMask.NameToLayer("Enemy");
        canHit = LayerMask.NameToLayer("CanHit");

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

        lightAtk = false;

        attackRate = 2;

        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamIncParry = 20;

        weaponHeld = false;
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
        int dmgToDeal = 0;
        // If the time elapsed since game started is more or equal to nextAttackTime, prevents spam
        if (Time.time >= nextAttackTime)
        {
            attacking = true;
            parryable = true;

            foreach (Collider2D hittableObj in playerAttack.GetObjectsHit())
            {
                if (lightAtk == true)
                {
                    UnityEngine.Debug.Log("Object Hit! (L)");
                    dmgToDeal = stats.lDmg;
                }
                else
                {
                    UnityEngine.Debug.Log("Object Hit! (H)");
                    dmgToDeal = stats.hDmg;
                }
                DealDamage(hittableObj, dmgToDeal);
                comboCount++;
            }
            
            attackCount++;
            
            switch (lightAtk)
            {
                case true:
                    UnityEngine.Debug.Log("Light Attack Performed");
                    // Decrease current stamina by stamDecLAttack
                    stats.AffectCurrentStamima(stamDecLAttack, "dec");
                    break;
                case false:
                    UnityEngine.Debug.Log("Heavy Attack Performed");
                    // Decrease current stamina by stamDecHAttack
                    stats.AffectCurrentStamima(stamDecHAttack, "dec");
                    break;
            }
            
            nextAttackTime = Time.time + 1f / attackRate;
        }
        attacking = false;
        parryable = false;
    }

    // Actually dealing the damage to the targeted objects
    private void DealDamage(Collider2D hittableObj, int dmgToDeal)
    {
        hittableObj.GetComponent<IDamageable>().TakeDamage(dmgToDeal);
    }

    // Performs a throw attack on the selected enemy, adds a force to the enemy object and deals damage
    public void Throw()
    {
        if (Time.time >= nextThrowTime)
        {
            // Throw feature will be configured here
            if (playerAttack.GetObjectsHit().Count == 0 || playerAttack.GetObjectsHit()[0].gameObject.layer != enemyLayer)
            {
                UnityEngine.Debug.Log("No valid object to throw");
            }
            else if (playerAttack.GetObjectsHit()[0] != null && playerAttack.GetObjectsHit()[0].gameObject.layer == enemyLayer)
            {
                throwing = true;
                attacking = true;
                
                GameObject toThrow = playerAttack.GetObjectsHit()[0].gameObject;
                // Add force to enemy rigid body 
                toThrow.GetComponent<EnemyScript>().rb.AddForce(toThrow.transform.right, ForceMode2D.Impulse);
                // Throw attack performed 
                UnityEngine.Debug.Log("Throw attack performed");
                stats.AffectCurrentStamima(stamDecThrow, "dec");
                
                attacking = false;

                nextThrowTime = Time.time + 1f / throwRate;
            }
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

    private void SwitchStyle()
    {
        canAttack = false;
        canDefend = false;
        playerStyleSwitch.SwitchStyle();
    }

    public void WeaponAttack()
    {
        //if (Time.time >= nextWAttackTime)
        //{
        //    // Weapon attack feature will be configured here
        //    if (Input.GetButtonDown("lAttack"))
        //    {
        //        UnityEngine.Debug.Log("Weapon Attack Initiated");
        //        attacking = true;
        //        parryable = true;

        //        // Light attack performed
        //        UnityEngine.Debug.Log("Light weapon attack performed");
        //        nextWAttackTime = Time.time + 1f / wAttackRate;
        //    }
        //    else if (Input.GetButtonDown("hAttack"))
        //    {
        //        UnityEngine.Debug.Log("Weapon Attack Initiated");
        //        attacking = true;
        //        parryable = true;

        //        // Heavy attack performed
        //        UnityEngine.Debug.Log("Heavy weapon attack performed");
        //        nextWAttackTime = Time.time + 1f / wAttackRate;
        //    }
        //    else if (Input.GetKeyDown(KeyCode.L))
        //    {
        //        UnityEngine.Debug.Log("Weapon Attack Initiated");
        //        attacking = true;

        //        // Gets the type of the weapon to determine which attack to perform
        //        UnityEngine.Debug.Log("Unique weapon attack performed");
        //        nextWAttackTime = Time.time + 1f / wAttackRate;
        //    }
        //}
        attacking = true;
        parryable = true;
        weapon.GetComponent<Weapon>().Attack();
        attacking = false;
        parryable = false;
    }

    // TEMPORARY
    public void DropWeapon()
    {
        if (weapon != null)
        {
            
            weapon.GetComponent<Weapon>().DropItem();
        }
    }
}
