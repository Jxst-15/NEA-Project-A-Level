using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    #region Script References
    private PlayerCombat combatScript;
    #endregion

    #region Variables
    [SerializeField] private int maxHealth, currentHealth;
    [SerializeField] private int maxStamina, currentStamina;
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
    }

    // Start is called before the first frame update
    void Start()
    {
        // Sets values for variables
        dead = false;
        maxHealth = 550;
        currentHealth = maxHealth;
        lDmg = 50;
        hDmg = 75;
        maxStamina = 200;
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        DeathCheck();
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
        StaminaCheck(currentStamina);
    }

    // Runs a check to make sure stamina does not go above or below allowed levels
    private void StaminaCheck(int stam)
    {
        if (currentStamina > maxStamina)
        {
            Debug.Log("Current stamina has exceeded the max value, setting to max value");
            currentStamina = maxStamina;
        }
        else if (currentStamina <= 0)
        {
            Debug.Log("Stamina is less than 0, cannot decrease further, setting value to 0");
            currentStamina = 0;
            //Debug.Log("Player can no longer block");
            //combatScript.canDefend = false;
            //combatScript.blocking = false;
        }
    }

    public void TakeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as a parameter
        currentHealth -= dmg;
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (currentHealth <= 0)
        {
            Death();
        }
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
