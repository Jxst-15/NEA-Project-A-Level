using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variables
    [SerializeField] private int maxHealth, currentHealth;

    [SerializeField] private int lDmg, hDmg;
    
    [SerializeField] private int maxStamina, currentStamina;
    #endregion

    #region Getters and Setters
    [SerializeField] private bool dead
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

    public void setLDmg(int dmg)
    {
        lDmg = dmg;
    }

    public void setHDmg(int dmg)
    {
        hDmg = dmg;
    }

    public void setCurrentStamina(int stam)
    {
        currentStamina = stam;
    }
    #endregion

    public void decCurrentHealth(int h)
    {
        currentHealth -= h;
    }

    public void affectCurrentStamima(int stam, string incOrDec) 
    {
        // Uses if else if statement to decide what to do with stamina value
        if (incOrDec == "Decrease")
        {
            currentStamina -= stam;
        }
        else if (incOrDec == "Increase")
        {
            currentStamina += stam;
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
        this.enabled = false;
        Debug.Log("Player was defeated");
    }
}
