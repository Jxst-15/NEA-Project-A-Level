using System.Linq;
using UnityEngine;

public class PlayerCombat : CharCombat
{
    #region Fields
    #region Script References
    private PlayerStats playerStats;
    private PlayerStyleSwitch playerStyleSwitch;
    public PlayerComboMeter comboMeter
    { get; private set; }
    private PlayerController playerController;
    private PlayerWCHandler playerWCHandler;

    private PlayerAttack playerAttack;
    private PlayerThrow playerThrow;
    private PlayerBlock playerBlock;

    public Weapon weaponScript
    { get; set; }
    #endregion

    #region Variables
    private float nextFTime; // Next finisher time

    // For determining whether 'H' is being held or pressed
    private float pressedTime;
    private bool keyHeld;
    #endregion
    
    private const float minHold = 0.2f; // Determines how long a button has to be held for to be classed as being held, used for blocking and parrying

    #region Getters and Setters
    public int stamDecFinish
    { get; set; }

    public bool canSwitch
    { get; set; }

    public int comboCount
    { get; set; }

    public int enemiesDefeated
    { get; set; }
    #endregion

    #endregion

    #region Unity Methods
    // Used for getting the required scripts
    void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerStyleSwitch = GetComponent<PlayerStyleSwitch>();
        comboMeter = GetComponent<PlayerComboMeter>();
        playerController = GetComponent<PlayerController>();
        playerWCHandler = GetComponent<PlayerWCHandler>();

        // Gets the PlayerAttack script from the attackBox GameObject
        playerAttack = attackBox.GetComponent<PlayerAttack>();

        playerThrow = throwBox.GetComponent<PlayerThrow>();

        // Gets the PlayerBlock script from the blockBox GameObject
        playerBlock = blockBox.GetComponent<PlayerBlock>();

    }

    protected override void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            // If player is able to attack
            if (canAttack == true)
            {
                // If there is no weapon
                if (playerWCHandler.weaponHeld == false)
                {
                    // weaponHeld = false;
                    AttackLogic();
                }
                else
                {
                    if (playerWCHandler.weapon != null)
                    {
                        playerWCHandler.WeaponAttackLogic();
                    }
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
        }
    }
    #endregion

    protected override void SetVariables()
    {
        parryBox.SetActive(false);

        canSwitch = true;

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
        stamDecFinish = 40;

        base.SetVariables();
    }

    public void ResetComboCount()
    {
        comboMeter.ResetCombo();
    }

    public void StartCombo()
    {
        comboMeter.ComboStart(comboCount);
    }

    protected override void AttackLogic()
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Finisher();
        }
    }
    protected override void Attack()
    {
        // If the time elapsed since game started is more or equal to nextAttackTime, prevents spam
        if (Time.time >= nextAttackTime)
        {
            int dmgToDeal;
            int toDecBy = 0;
        
            attacking = true;
            parryable = true;

            // Damages all enemies in the collider
            foreach (Collider2D hittableObj in playerAttack.GetObjectsHit().ToList())
            {
                if (lightAtk == true)
                {
                    dmgToDeal = playerStats.lDmg;
                    toDecBy = stamDecLAttack;
                }
                else
                {
                    dmgToDeal = playerStats.hDmg;
                    toDecBy = stamDecHAttack;
                }
                DealDamage(hittableObj, dmgToDeal);
                comboCount++;
                StartCombo();
            }
            playerStats.AffectCurrentStamima(toDecBy, "dec");

            attacking = false;
            parryable = false;
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    protected override void AttackWhenBlocking(Collider2D hittableObj)
    {
        throw new System.NotImplementedException();
    }

    public void Finisher()
    {
        if (Time.time >= nextFTime && comboCount >= comboMeter.GetComboForF() && playerAttack.GetObjectsHit().ToList()[0] != null)
        {
            Collider2D hittableObj = playerAttack.GetObjectsHit()[0]; // Only damages the first enemy in the list
            DealDamage(hittableObj, playerStats.fDmg);

            comboMeter.ResetCombo();
            Debug.Log("Used Finisher");

            playerStats.AffectCurrentStamima(stamDecFinish, "dec");

            nextFTime = Time.time + 7;
        }
    }

    protected override void Throw()
    {
        // If the list in playerThrow has elements in it then execute following
        if (Time.time >= nextThrowTime && playerThrow.ObjectsHitCount() != 0)
        {
            attacking = true;

            // Calls throw method
            playerThrow.Throw();
            playerStats.AffectCurrentStamima(stamDecThrow, "dec");
            attacking = false;

            // For cooldowns
            nextThrowTime = Time.time + 1f / throwRate;
        }
    }

    protected override void DefendLogic()
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
                Debug.Log("Parry initiated");
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

    protected override void Block()
    {
        // Block feature will be configured here
        blocking = true;
        canAttack = false;
        playerBlock.Block();
    }

    protected override void Parry()
    {
        Debug.Log("Parry");
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
}
