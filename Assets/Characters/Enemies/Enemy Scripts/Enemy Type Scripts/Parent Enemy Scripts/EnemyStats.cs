using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Script References
    private EnemyScript enemyScript;
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
    public int lDmg
    { get; set; }
    public int hDmg
    { get; set; }
    public int uDmg
    { get; set; }
    public bool dead
    { get; set; }
    #endregion

    void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
    }

    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        // Sets values for variables depending on tag
        SetVariables();
    }
    
    private void SetVariables()
    {
        type = enemyScript.type;
        switch (type)
        {
            case "NormalEnemies":
                maxHealth = 400;
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
                lDmg = 60;
                hDmg = 80;
                uDmg = 100;
                maxStamina = 100;
                vSpeed = 1;
                hSpeed = 2;
                vRunSpeed = 3;
                hRunSpeed = 4;
                break;
        }
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }    

    public void EStatsUpdate()
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

    public void setStamina(int stam)
    {
        currentStamina = stam;
    }

    public void setCurrentStamina(int stam)
    {
        currentStamina = stam;
    }
    #endregion

    public void affectCurrentStamima(int stam, string incOrDec)
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

    public void StaminaCheck(int stam)
    {
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        else if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    public void takeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as the parameter
        currentHealth -= dmg;
        DeathCheck();
    }

    private void DeathCheck()
    {
        if (currentHealth <= 0)
        {
            death();
        }
    }

    private void death()
    {
        dead = true;
        // Disable the game object 
        gameObject.SetActive(false);
        // Destroy the game object helps to manage memory and declutter screen
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }
}
