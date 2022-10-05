using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Variables
    [SerializeField] private int maxHealth, currentHealth;

    [SerializeField] private int lDmg, hDmg, uDmg;
    
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
        maxHealth = 400;
        currentHealth = maxHealth;
        lDmg = 40;
        hDmg = 60;
        uDmg = 80;
        maxStamina = 200;
        currentStamina = maxStamina;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if enemy's current health is 0 or lower
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

    public void takeDamage(int dmg)
    {
        // currentHealth is affected by the damage given as the parameter
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
        // Destroy the game object helps to manage memory and declutter screen
        Destroy(gameObject);
        Debug.Log("Enemy Died");
    }
}
