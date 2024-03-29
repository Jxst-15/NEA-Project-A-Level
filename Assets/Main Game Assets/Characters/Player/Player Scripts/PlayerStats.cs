using UnityEngine;

public class PlayerStats : CharStats
{
    #region Fields
    #region Script References
    [SerializeField] private PlayerCombat combatScript;
    [SerializeField] private PlayerController controllerScript;
    #endregion

    #region Getters and Setters
    public int fDmg
    { get; private set; }
    public bool dead
    { get; private set; }
    #endregion
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        combatScript = GetComponent<PlayerCombat>();
        controllerScript = GetComponent<PlayerController>();
    }
    #endregion

    protected override void SetVariables()
    {
        // Sets values for variables
        maxHealth = 900;
        lDmg = 50;
        hDmg = 75;
        fDmg = 100;
        maxStamina = 450;
        nextRegen = 0f;

        vSpeed = 3;
        hSpeed = 4;
        vRunSpeed = 5;
        hRunSpeed = 6;

        unstunCooldown = 5;

        currentHealth = PlayerData.instance.currentHealth;
        currentStamina = PlayerData.instance.currentStamina;
    }

    public void Heal(int toHealBy)
    {
        currentHealth += toHealBy;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    protected override void StaminaRegen()
    {
        if (Time.time >= nextRegen)
        {
            int toIncBy = 0;
            
            // Following if block is to determine the speed and the amount to regen stamina by
            if (currentStamina < maxStamina && currentStamina > maxStamina / 2)
            {
                toIncBy = 10;
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

    public override void TakeDamage(int dmg, bool weapon)
    {
        base.TakeDamage(dmg, weapon);
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

        controllerScript.StopMovement();

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
        dead = true;

        gameObject.SetActive(false);
    }
}
