using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
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
        if (currentHealth <= 0)
        {
            death();
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
            Debug.Log("Current stamina has exceeded the max value, setting to max value");
            currentStamina = maxStamina;
        }
        else if (currentStamina < 0)
        {
            Debug.Log("Stamina is less than 0, cannot decrease further, setting value to 0");
            currentStamina = 0;
        }
    }

    public void takeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as a parameter
        currentHealth -= dmg;
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
        Debug.Log("Player was defeated");
    }
}
