using UnityEngine;

public class PlayerStats : CharStats
{
    #region W/O Inheritance
    /*
    #region Script References
    [SerializeField] private PlayerCombat combatScript;
    [SerializeField] private PlayerController controllerScript;
    #endregion

    #region Variables
    [SerializeField] private int maxHealth, currentHealth;
    [SerializeField] private int maxStamina, currentStamina;

    private float nextRegen;
    
    // [SerializeField] private bool stun;
    private float tillUnstun;
    #endregion

    #region Getters and Setters
    public int lDmg
    { get; set; }
    public int hDmg
    { set; get; }
    public bool stun
    { get; private set; }
    public bool dead
    { get; set; }
    #endregion

    void Awake()
    {
        combatScript = GetComponent<PlayerCombat>();
        controllerScript = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is not dead
        if (DeathCheck() == false)
        {          
            StaminaRegen();
            if (stun == true)
            {
                WaitForUnstun();
            }
        }
    }

    #region Set Methods
    public void setHealth(int h)
    {
        maxHealth = h;
    }

    public void setCurrentHealth(int h)
    {
        currentHealth = h;
    }

    public void setCurrentStamina(int stam)
    {
        currentStamina = stam;
    }
    #endregion

    private void SetVariables()
    {
        // Sets values for variables
        dead = false;
        maxHealth = 550;
        currentHealth = maxHealth;
        lDmg = 50;
        hDmg = 75;
        maxStamina = 200;
        currentStamina = maxStamina;
        nextRegen = 0f;
    }

    // Parameters indicate the amount to affect stamina by and whether it is an increase or decrease
    public void AffectCurrentStamima(int stam, string incOrDec) 
    {
        if (incOrDec == "dec")
        {
            currentStamina -= stam;
        }
        else if (incOrDec == "inc")
        {
            currentStamina += stam;
        }
        StaminaCheck();
    }

    // Runs a check to make sure stamina does not go above or below allowed levels
    private void StaminaCheck()
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina <= 0)
        {
            currentStamina = 0;
            if (stun != true)
            {
                Stun();
            }
        }
    }

    // WIP
    private void StaminaRegen()
    {
        if (Time.time >= nextRegen)
        {
            int toIncBy = 0;
            // Following if block is to determine the speed and the amount to regen stamina by
            if (currentStamina < maxStamina && currentStamina > maxStamina / 2)
            {
                toIncBy = 5;
                nextRegen = Time.time + 5f;
            }
            else if (currentStamina < maxStamina && currentStamina <= maxStamina / 2)
            {
                toIncBy = 20;
                nextRegen = Time.time + 2.5f;
            }
            // If the time elapsed is more than or equal to whenever the next regen time is, increase stamina by set amount
            AffectCurrentStamima(toIncBy, "inc");
        }
    }

    public void TakeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as a parameter
        currentHealth -= dmg;
        combatScript.ResetComboCount();
        // DeathCheck();
    }

    //WIP
    public void Stun()
    {
        Debug.Log("Stunned");
        stun = true;
        
        // Makes it so player can no longer attack, defend, and make it so their block is removed
        combatScript.canAttack = false;
        combatScript.canDefend = false;
        combatScript.blocking = false;

        // Disables player movement
        controllerScript.canMove = false;
        
        // Sets the time that the player will be unstunned
        tillUnstun = Time.time + 5f;
    }

    // WIP
    private void WaitForUnstun()
    {
        // If time elapsed is more than the time that is set to unstun player
        if (Time.time > tillUnstun)
        {
            // Unstun the player and allow player to act again
            Debug.Log("Unstunned");
            combatScript.canAttack = true;
            combatScript.canDefend = true;
            combatScript.blocking = true;

            controllerScript.canMove = true;
            stun = false;
        }

    }

    private bool DeathCheck()
    {
        if (currentHealth <= 0)
        {
            Death();
            return true;
        }
        return false;
    }

    // Runs when this game object has been defeated
    private void Death()
    {
        dead = true;
        // Disable the game object
        gameObject.SetActive(false);
        Debug.Log("Player was defeated");
    }
    */
    #endregion
    
    #region Script References
    [SerializeField] private PlayerCombat combatScript;
    [SerializeField] private PlayerController controllerScript;
    #endregion

    protected override void Awake()
    {
        combatScript = GetComponent<PlayerCombat>();
        controllerScript = GetComponent<PlayerController>();
    }

    protected override void SetVariables()
    {
        // Sets values for variables
        maxHealth = 550;
        lDmg = 50;
        hDmg = 75;
        maxStamina = 200;
        nextRegen = 0f;
        base.SetVariables();
    }

    // WIP
    protected override void StaminaRegen()
    {
        if (Time.time >= nextRegen)
        {
            int toIncBy = 0;
            // Following if block is to determine the speed and the amount to regen stamina by
            if (currentStamina < maxStamina && currentStamina > maxStamina / 2)
            {
                toIncBy = 5;
                nextRegen = Time.time + 5f;
            }
            else if (currentStamina < maxStamina && currentStamina <= maxStamina / 2)
            {
                toIncBy = 20;
                nextRegen = Time.time + 2.5f;
            }
            // If the time elapsed is more than or equal to whenever the next regen time is, increase stamina by set amount
            AffectCurrentStamima(toIncBy, "inc");
        }
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        combatScript.ResetComboCount();
    }

    public override void Stun()
    {
        Debug.Log("Stunned");

        // Makes it so player can no longer attack, defend, and make it so their block is removed
        combatScript.canAttack = false;
        combatScript.canDefend = false;
        combatScript.blocking = false;

        // Disables player movement
        controllerScript.canMove = false;

        base.Stun();
    }

    protected override void WaitForUnstun()
    {
        // If time elapsed is more than the time that is set to unstun player
        if (Time.time > tillUnstun)
        {
            // Unstun the player and allow player to act again
            Debug.Log("Unstunned");
            combatScript.canAttack = true;
            combatScript.canDefend = true;
            combatScript.blocking = true;

            controllerScript.canMove = true;
            stun = false;
        }

    }

    // Runs when this game object has been defeated
    protected override void Death()
    {
        // Disable the game object
        gameObject.SetActive(false);
        Debug.Log("Player was defeated");
    }
}
