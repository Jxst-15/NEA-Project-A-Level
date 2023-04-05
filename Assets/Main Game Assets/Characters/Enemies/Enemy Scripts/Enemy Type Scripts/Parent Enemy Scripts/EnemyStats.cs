using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharStats
{
    #region Fields
    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyCombat enemyCombat;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAI enemyAI;

    [SerializeField] private FlashScript flashScript;
    #endregion

    #region Script Reference Variables
    [SerializeField] private string type;
    #endregion

    #region Variables
    private string weakTo;
    #endregion

    #region Getters and Setters
    public float attackRate
    { get; set; }
    public int uDmg
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

    // Start is called before the first frame update
    protected override void Start()
    {
        StartInit();
    }

    protected void OnEnable()
    {
        StartInit();
    }
    #endregion

    protected void StartInit()
    {
        enemyScript = GetComponent<EnemyScript>();
        flashScript = GetComponent<FlashScript>();
        base.Start();
    }

    protected override void SetVariables()
    {
        type = enemyScript.type;
        // Based on the tag of the game object, sets the stats accordingly
        switch (type)
        {
            case "NormalEnemies":
                maxHealth = 300;
                attackRate = 2;
                lDmg = 30;
                hDmg = 50;
                uDmg = 70;

                maxStamina = 200;
                regenCooldown = 5;

                vSpeed = 2;
                hSpeed = 3;
                vRunSpeed = 4;
                hRunSpeed = 5;

                weakTo = "Iron Fist";
                break;
            case "NimbleEnemies":
                maxHealth = 200;
                attackRate = 3;
                lDmg = 10;
                hDmg = 20;
                uDmg = 40;

                maxStamina = 300;
                regenCooldown = 3;

                vSpeed = 3;
                hSpeed = 4;
                vRunSpeed = 5;
                hRunSpeed = 6;

                weakTo = "Grass Style";
                break;
            case "BulkyEnemies":
                maxHealth = 500;
                attackRate = 0.5f;
                lDmg = 50;
                hDmg = 70;
                uDmg = 90;

                maxStamina = 100;
                regenCooldown = 7;

                vSpeed = 1;
                hSpeed = 2;
                vRunSpeed = 3;
                hRunSpeed = 4;

                weakTo = "Boulder Style";
                break;
            case "BossEnemies":
                maxHealth = 1000;
                attackRate = 1f;
                lDmg = 80;
                hDmg = 95;
                uDmg = 115;

                maxStamina = 400;
                regenCooldown = 10;

                vSpeed = 2;
                hSpeed = 3;
                vRunSpeed = 5;
                hRunSpeed = 6;

                weakTo = "None";
                break;
        }

        if (type != "BossEnemies")
        {
            toIncBy = 5;
            unstunCooldown = 7;
        }
        else
        {
            toIncBy = 15;
            unstunCooldown = 10;
        }

        base.SetVariables();
    }

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
        if (PlayerStyleSwitch.fightStyle == weakTo && enemyScript.target.GetComponent<PlayerCombat>().weaponHeld == false && weapon == false)
        {
            int weakToAddOn = 10;
            dmg += weakToAddOn; 
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
