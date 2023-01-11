using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    #region Script References
    private PlayerCombat combatScript;
    private PlayerController controllerScript;
    #endregion

    #region Variables
    [SerializeField] private int maxHealth, currentHealth;
    [SerializeField] private int maxStamina, currentStamina;

    private float nextRegen;
    
    private bool stun;
    private float timeStunnedFor;
    #endregion

    #region Getters and Setters
    public int lDmg
    { get; set; }
    public int hDmg
    { set; get; }
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
        if (DeathCheck() == false)
        {
            if (stun == false)
            {
                StaminaRegen();
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

    public void AffectCurrentStamima(int stam, string incOrDec) 
    {
        if (currentStamina != maxStamina && currentStamina < maxStamina || currentStamina == maxStamina)
        {
            if (incOrDec == "dec")
            {
                currentStamina -= stam;
            }
            else if (incOrDec == "inc")
            {
                currentStamina += stam;
            }
        }
        StaminaCheck();
    }

    // Runs a check to make sure stamina does not go above or below allowed levels
    private void StaminaCheck()
    {
        if (currentStamina > maxStamina)
        {
            Debug.Log("Current stamina has exceeded the max value, setting to max value");
            currentStamina = maxStamina;
        }
        else if (currentStamina <= 0)
        {
            // Debug.Log("Stamina is less than 0, cannot decrease further, setting value to 0");
            currentStamina = 0;
            // Debug.Log("Player can no longer block");
            Stunned();
        }
    }

    // WIP
    private void StaminaRegen()
    {
        if (Time.time >= nextRegen)
        {
            int toIncBy = 0;
                if (currentStamina < maxStamina && currentStamina > maxStamina / 2)
                {
                    // and set next regen time to time elapsed + 3.5 seconds
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
        DeathCheck();
    }

    // WIP
    public void Stunned()
    {
        stun = true;
        timeStunnedFor = Time.time + 5f;

        if (Time.time < timeStunnedFor)
        {
            combatScript.canAttack = false;
            combatScript.canDefend = false;
            combatScript.blocking = false;

            controllerScript.canMove = false;
        }
        else
        {
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
}
