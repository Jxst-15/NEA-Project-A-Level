using UnityEngine;

public class PlayerStats : CharStats
{
    #region Fields
    #region Script References
    [SerializeField] private PlayerCombat combatScript;
    [SerializeField] private PlayerController controllerScript;
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
