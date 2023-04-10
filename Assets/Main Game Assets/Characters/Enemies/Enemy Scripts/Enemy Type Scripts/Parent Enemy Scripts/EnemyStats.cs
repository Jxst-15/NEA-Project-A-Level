using UnityEngine;

public abstract class EnemyStats : CharStats
{
    #region Fields
    #region Script References
    [SerializeField] protected EnemyScript enemyScript;
    [SerializeField] protected EnemyCombat enemyCombat;
    [SerializeField] protected EnemyMovement enemyMovement;
    [SerializeField] protected EnemyAI enemyAI;

    [SerializeField] protected FlashScript flashScript;
    #endregion

    #region Variables
    protected string weakTo; // Indicates which player fighting style the enemy is weak to

    protected int toIncBy; // To increase stamina by
    #endregion

    #region Getters and Setters
    public float attackRate
    { get; set; }
    public int uDmg // Ultimate damage
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        enemyCombat = GetComponent<EnemyCombat>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();
    }

    // Called when the gameobject is enables and active
    protected virtual void OnEnable()
    {
        enemyScript = GetComponent<EnemyScript>();
        flashScript = GetComponent<FlashScript>();
        base.Start();
    }
    #endregion

    protected override void StaminaRegen()
    {
        // If the time elapsed is more than or equal to whenever the next regen time is and currentStamina is less than the max, increase stamina by set amount
        if (Time.time >= nextRegen && currentStamina < maxStamina)
        {
            // Sets the next time that the enemy regens stamina
            nextRegen = Time.time + regenCooldown;

            AffectCurrentStamima(toIncBy, "inc");
        }
    }

    public override void TakeDamage(int dmg, bool weapon)
    {
        // If the enemy is weak to the players; fighting style and the enemy is not holding a weapon and the damage is not being done by a weapon
        if (PlayerStyleSwitch.fightStyle == weakTo && enemyScript.target.GetComponent<PlayerWCHandler>().weaponHeld == false && weapon == false)
        {
            int weakToAddOn = 10;
            dmg += weakToAddOn; // Add on extra damage to do to enemy
        }
        base.TakeDamage(dmg, weapon);
        flashScript.Flash(flashScript.GetFlashMaterial(0));
    }

    public override void Stun()
    {
        enemyCombat.canAttack = false;
        enemyCombat.canDefend = false;
        enemyCombat.blocking = false;

        enemyMovement.canMove = false;
        enemyMovement.StopMovement();

        base.Stun();
    }

    protected override void WaitForUnstun()
    {
        if (Time.time > tillUnstun)
        {
            enemyCombat.canAttack = true;
            enemyCombat.canDefend = true;
            enemyCombat.blocking = true;

            enemyMovement.canMove = true;

            stun = false;
        }
    }

    protected override void Death()
    {
        // As the enemy has no health left, switch states to Inactive
        enemyAI.fsm.MoveStates(EnemyCommands.NoHealth);

        // On death, gives the player points and increases their number of enemies defeated
        enemyScript.OnDeath();

        flashScript.Flash(flashScript.GetFlashMaterial(2));

        // Destroying the game object helps to manage memory and declutter screen
        Destroy(gameObject);
    }
    
}
