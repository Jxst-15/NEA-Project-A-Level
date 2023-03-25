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
                maxHealth = 400;
                attackRate = 2;
                lDmg = 40;
                hDmg = 60;
                uDmg = 80;

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
                lDmg = 20;
                hDmg = 30;
                uDmg = 50;

                maxStamina = 300;
                regenCooldown = 3;

                vSpeed = 3;
                hSpeed = 4;
                vRunSpeed = 5;
                hRunSpeed = 6;

                weakTo = "Grass Style";
                break;
            case "BulkyEnemies":
                maxHealth = 600;
                attackRate = 0.5f;
                lDmg = 60;
                hDmg = 80;
                uDmg = 100;

                maxStamina = 100;
                regenCooldown = 7;

                vSpeed = 1;
                hSpeed = 2;
                vRunSpeed = 3;
                hRunSpeed = 4;

                weakTo = "Boulder Style";
                break;
            case "BossEnemies":
                maxHealth = 700;
                attackRate = 0.5f;
                lDmg = 70;
                hDmg = 90;
                uDmg = 110;

                maxStamina = 400;
                regenCooldown = 10;

                vSpeed = 2;
                hSpeed = 3;
                vRunSpeed = 4;
                hRunSpeed = 5;
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

    public override void TakeDamage(int dmg)
    {
        if (PlayerStyleSwitch.fightStyle == weakTo)
        {
            int weakToAddOn = 10;
            dmg += weakToAddOn; 
        }
        base.TakeDamage(dmg);
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

        // On death, gives the player points
        enemyScript.GivePoints();

        flashScript.Flash(flashScript.GetFlashMaterial(2));
        
        // Destroying the game object helps to manage memory and declutter screen
        Destroy(gameObject);
    }
}
