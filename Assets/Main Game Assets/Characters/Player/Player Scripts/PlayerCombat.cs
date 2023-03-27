using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour, ICharacterCombat, IWeaponHandler
{
    #region Fields
    #region Script References
    public PlayerStats stats
    { get; private set; }
    public PlayerStyleSwitch playerStyleSwitch
    { get; private set; }
    public PlayerComboMeter comboMeter
    { get; private set; }
    public PlayerController playerController
    { get; private set; }
    
    public PlayerAttack playerAttack
    { get; private set; }
    public PlayerThrow playerThrow
    { get; private set; }
    public PlayerBlock playerBlock
    { get; private set; }
    #endregion

    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] public GameObject weapon;

    public GameObject attackBox, blockBox, parryBox, throwBox;
    
    // LayerMasks help to identify which objects can be hit
    public LayerMask enemyLayer, canHit;

    [SerializeField] private float attackRange;
    [SerializeField] private int attackCount;
    private float nextAttackTime;
    
    // How many attacks player has performed without getting hit (Field)
    [SerializeField] private int comboCount;

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
    public bool canSwitch
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
    #endregion

    #region Unity Methods
    // Used for getting the required scripts
    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerStyleSwitch = GetComponent<PlayerStyleSwitch>();
        comboMeter = GetComponent<PlayerComboMeter>();
        playerController = GetComponent<PlayerController>();
        
        // Gets the PlayerAttack script from the attackBox GameObject
        playerAttack = attackBox.GetComponent<PlayerAttack>(); 
        
        // Gets the PlayerBlock script from the blockBox GameObject
        playerBlock = blockBox.GetComponent<PlayerBlock>();

        playerThrow = throwBox.GetComponent<PlayerThrow>();
        
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
            // ComboMeter();

            // If player is able to attack
            if (canAttack == true)
            {
                // If there is no weapon
                if (weapon == null)
                {
                    weaponHeld = false;
                    AttackLogic();
                }
                else
                {
                    WeaponAttackLogic();
                }
            }

            // If player is able to defend
            if (canDefend == true)
            {
                DefendLogic();
            }

            // If LeftShift is held then will call SwitchStyle method
            if (Input.GetKey(KeyCode.LeftShift))
            {
                SwitchStyle();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                // Sets game time back to normal
                Time.timeScale = 1f;
                playerController.playerAction.canInteract = true;
                canAttack = true;
                canDefend = true;

                playerStyleSwitch.StyleSwitcher.SetActive(false);
            }

            // TEMPORARY
            if (Input.GetKeyDown(KeyCode.B))
            {
                DropWeapon();
            }
        }
    }
    #endregion

    private void SetVariables()
    {
        // blockBox.SetActive(false);
        parryBox.SetActive(false);

        enemyLayer = LayerMask.NameToLayer("Enemy");
        canHit = LayerMask.NameToLayer("CanHit");

        canAttack = true;
        canDefend = true;
        canSwitch = true;
        attacking = false;
        blocking = false;
        throwing = false;
        parryable = false;

        attackRange = 1.5f;
        attackCount = 0;
        nextAttackTime = 0f;

        throwRate = 0.2f;
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

    private void AttackLogic()
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
            
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                Throw();
            }
    }

    private void DefendLogic()
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
                // blockBox.SetActive(false);
                blocking = false;
                canAttack = true;
            }
    }
 
    public void ResetComboCount()
    {
        comboCount = 0;
        comboMeter.inCombo = false;
    }

    public void StartCombo()
    {
        comboMeter.ComboStart();
    }

    public void Attack()
    {
        int dmgToDeal;
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
                StartCombo();
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
        hittableObj.GetComponent<IDamageable>().TakeDamage(dmgToDeal, false);
    }

    // Performs a throw attack on the selected enemy, configured in the PlayerThrow class
    public void Throw()
    {
        // If the list in playerThrow has elements in it then execute following
        if (Time.time >= nextThrowTime && playerThrow.ObjectsHitCount() != 0)
        {
            attacking = true;
                
            // Calls throw method
            playerThrow.Throw();
            stats.AffectCurrentStamima(stamDecThrow, "dec");
            attacking = false;

            // For cooldowns
            nextThrowTime = Time.time + 1f / throwRate;
        }
        else if (Time.time < nextThrowTime)
        {
            UnityEngine.Debug.Log("Throw is on cooldown");
        }
        // If there are no elements in the list
        else if (playerThrow.ObjectsHitCount() == 0)
        {
            UnityEngine.Debug.Log("No valid enemy to throw");
        }
    }

    // Negates all incoming damage from enemies in direction player facing, deals chip damage and stamina to player
    public void Block()
    {
        // Block feature will be configured here
        blocking = true;
        canAttack = false;
        playerBlock.Block();
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
        if (canSwitch)
        {
            canAttack = false;
            canDefend = false;
            playerController.playerAction.canInteract = false;
            playerStyleSwitch.SwitchStyle();
        }
    }

    #region Weapon Code
    private void WeaponAttackLogic()
    {
        if (Input.GetButtonDown("lAttack") || Input.GetButtonDown("hAttack") || Input.GetKeyDown(KeyCode.L))
        {
            bool unique = false;
            
            if (Input.GetButtonDown("lAttack"))
            {
                lightAtk = true;
            }
            else if (Input.GetButtonDown("hAttack"))
            {
                lightAtk = false;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                unique = true;
            }
            
            WeaponAttack(unique);
        }       
    }

    public void WeaponAttack(bool unique)
    {
        attacking = true;
        parryable = true;
        if (!unique)
        {
            if (weapon.GetComponent<Weapon>().Attack(lightAtk) == true)
            {
                // Get the number of objects hit by attack and increase comboCount by amount
                comboCount += weapon.GetComponent<Weapon>().weaponAttack.GetObjectsHit().Count;
            }
        }
        else
        {
            if (weapon.GetComponent<Weapon>().UniqueAttack() == true)
            {
                // Get the number of objects hit by attack and increase comboCount by amount
                comboCount += weapon.GetComponent<Weapon>().uniqueWeaponAttack.GetObjectsHit().Count; // DOESNT WORK FOR THROWABLES
            }
        }
        attackCount++;
        
        attacking = false;
        parryable = false;
    }

    public void SetWeaponToNull()
    {
        weapon = null;
    }

    // TEMPORARY
    public void DropWeapon()
    {
        if (weapon != null)
        {         
            weapon.GetComponent<Weapon>().DropItem();
            weapon = null;
            weaponHeld = false;
            
            canSwitch = true;
        }
    }
    #endregion
}
