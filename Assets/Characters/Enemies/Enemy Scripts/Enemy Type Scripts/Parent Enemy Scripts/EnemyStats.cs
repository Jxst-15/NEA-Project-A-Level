using UnityEngine;

public class EnemyStats : MonoBehaviour, IDamageable
{
    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private FlashScript flashScript;
    #endregion

    #region Script Reference Variables
    [SerializeField] private string type;
    #endregion

    #region Variables
    [SerializeField] private int maxHealth, currentHealth;
    [SerializeField] private int maxStamina, currentStamina;
    #endregion

    #region Getters and Setters
    public int vSpeed
    { get; set; }
    public int hSpeed
    { get; set; }
    public int vRunSpeed
    { get; set; }
    public int hRunSpeed
    { get; set; }
    public float attackRate
    { get; set; }
    public int lDmg
    { get; set; }
    public int hDmg
    { get; set; }
    public int uDmg
    { get; set; }
    public bool stun
    { get; set; }
    public bool dead
    { get; set; }
    #endregion

    void Awake()
    {
        dead = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        flashScript = GetComponent<FlashScript>();
        SetVariables();
    }

    // Update is called once per frame
    void Update()
    {
        EStatsUpdate();
    }

    private void SetVariables()
    {
        type = enemyScript.type;
        // Based on the tag of the game object, sets the stats accordingly
        switch (type)
        {
            case "NormalEnemies":
                maxHealth = 400;
                attackRate = 2;
                lDmg = 40;
                hDmg = 60;
                uDmg = 80;
                maxStamina = 200;
                vSpeed = 2;
                hSpeed = 3;
                vRunSpeed = 4;
                hRunSpeed = 5;
                break;
            case "NimbleEnemies":
                maxHealth = 200;
                attackRate = 3;
                lDmg = 20;
                hDmg = 30;
                uDmg = 50;
                maxStamina = 300;
                vSpeed = 3;
                hSpeed = 4;
                vRunSpeed = 5;
                hRunSpeed = 6;
                break;
            case "BulkyEnemies":
                maxHealth = 600;
                attackRate = 0.5f;
                lDmg = 60;
                hDmg = 80;
                uDmg = 100;
                maxStamina = 100;
                vSpeed = 1;
                hSpeed = 2;
                vRunSpeed = 3;
                hRunSpeed = 4;
                break;
            case "BossEnemies":
                maxHealth = 700;
                attackRate = 0.5f;
                lDmg = 70;
                hDmg = 90;
                uDmg = 110;
                maxStamina = 400;
                vSpeed = 2;
                hSpeed = 3;
                vRunSpeed = 4;
                hRunSpeed = 5;
                break;
        }
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }    

    public void EStatsUpdate()
    {
        if (DeathCheck() == false)
        {
            // Regen enemy stamina
            // Check for stun
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

    public void setLDmg(int dmg)
    {
        lDmg = dmg;
    }

    public void setHDmg(int dmg)
    {
        hDmg = dmg;
    }

    public void setUDmg(int dmg)
    {
        uDmg = dmg;
    }

    public void setMaxStamina(int stam)
    {
        maxStamina = stam;
    }

    public void setCurrentStamina(int stam)
    {
        currentStamina = stam;
    }
    #endregion

    public void AffectCurrentStamima(int stam, string incOrDec)
    {
        // if (currentStamina != maxStamina && currentStamina < maxStamina || currentStamina == maxStamina)
        // {
        if (incOrDec == "dec")
        {
            currentStamina -= stam;
        }
        else if (incOrDec == "inc")
        {
            currentStamina += stam;
        }
        // }
        StaminaCheck(currentStamina);
    }

    // Runs a check to make sure stamina does not go above or below allowed levels
    private void StaminaCheck(int stam)
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
                // Stun the enemy
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as the parameter
        currentHealth -= dmg;
        flashScript.Flash();
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
        // enemyScript.points = points;
        enemyScript.GivePoints();
        flashScript.Flash();
        // Disable the game object 
        gameObject.SetActive(false);
        // Destroying the game object helps to manage memory and declutter screen
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }
}
