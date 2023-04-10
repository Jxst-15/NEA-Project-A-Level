using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyCombat : CharCombat
{
    #region Fields
    #region Object References
    private readonly System.Random rng = new System.Random();
    #endregion

    #region Script References
    [SerializeField] protected EnemyScript enemyScript;
    [SerializeField] protected EnemyMovement enemyMovement;
    [SerializeField] protected EnemyAI enemyAI;
    [SerializeField] protected EnemyStats enemyStats;
    [SerializeField] protected EnemyAttack enemyAttack;
    [SerializeField] protected EnemyWCHandler enemyWCHandler;
    [SerializeField] protected FlashScript flashScript;

    [SerializeField] protected PlayerCombat targetAttackStatus;
    [SerializeField] protected PlayerBlock targetBlockStats;
    [SerializeField] protected PlayerStats targetStats;
    #endregion

    #region Target
    protected GameObject target;
    #endregion

    #region Variables
    protected float nextUATime;
    protected bool doingUnblockable;

    // Variables for affecting stat factors
    [SerializeField] protected int stamDecUAttack;
    #endregion
    
    protected const float uARate = 0.1f; // Ultimate attack rate

    #region Getters and Setters
    public int stamDecBlock
    { get; set; }
    
    public bool gettingBlocked
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        SetVariables();      
        
        enemyScript = GetComponent<EnemyScript>();
        target = enemyScript.target;

        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAttack = attackBox.GetComponent<EnemyAttack>();
        enemyWCHandler = GetComponent<EnemyWCHandler>();
        flashScript = GetComponent<FlashScript>();

        targetAttackStatus = target.GetComponent<PlayerCombat>();
        targetBlockStats = target.GetComponentInChildren<PlayerBlock>();
        targetStats = target.GetComponent<PlayerStats>();   

        attackRate = enemyStats.attackRate;

        attackBox.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (PauseMenu.isPaused == false || enemyStats.stun != true)
        {
            switch (enemyAI.fsm.currentState.thisStateID == EnemyStates.Attacking)
            {
                case true:
                    if (enemyWCHandler.weapon == null && enemyWCHandler.weaponHeld == false)
                    {
                        attackBox.SetActive(true);
                        ProbabilityOfActions();
                    }
                    else
                    {
                        enemyWCHandler.WeaponAttackLogic();
                    }
                    break;
                default:
                    attackBox.SetActive(false);
                    break;
            }
        }
    }
    #endregion

    protected override void SetVariables()
    {
        nextUATime = 0f;

        throwRate = 0.3f;
        nextThrowTime = 0f;

        stamDecUAttack = 30;
        stamDecBlock = 10;

        base.SetVariables();
    }

    protected virtual void ProbabilityOfActions()
    {       
        // To determine which action to do
        int randNum = rng.Next(1, 11);
        if (1 <= randNum && randNum <= 7 && canAttack == true)
        {
            AttackLogic();
        }
        else if (randNum == 8 || randNum == 9 && targetAttackStatus.attacking == true && canDefend == true)
        {
            Block();
        }
        else if (randNum == 10 && targetAttackStatus.parryable == true && canDefend == true)
        {
            Parry();
        }
    }

    protected override void AttackLogic()
    {
        if (Time.time >= nextAttackTime)
        {
            attacking = true;
            if (doingUnblockable != true)
            {
                int randNum = rng.Next(1, 11);

                if (1 <= randNum && randNum <= 8)
                {
                    Attack();
                }
                else if (randNum == 9)
                {
                    StartCoroutine(UnblockableAttack());
                }
                else 
                {
                    Throw();
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    protected override void Attack()
    {
        int randNum = rng.Next(1, 9);
        foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit().ToList())
        {
            if (hittableObj != null)
            {
                DecideGettingBlocked(hittableObj, randNum);
            }
        }
    }

    // Switch statement to see if the enemy is being blocked or not
    protected void DecideGettingBlocked(Collider2D hittableObj, int randNum)
    {
        switch (gettingBlocked)
        {
            case false:
                int dmgToDeal = 0;
                if (1 <= randNum && randNum <= 6)
                {
                    lightAtk = true;
                    dmgToDeal = enemyStats.lDmg;
                    Debug.Log("Light E");
                }
                else if (randNum == 7 || randNum == 8)
                {
                    lightAtk = false;
                    dmgToDeal = enemyStats.hDmg;
                    Debug.Log("Heavy E");
                }
                DealDamage(hittableObj, dmgToDeal);
                break;
            case true:
                AttackWhenBlocking(hittableObj);
                break;
        }
    }

    protected override void AttackWhenBlocking(Collider2D hittableObj)
    {
        // The following decreases the targets current stamina and deals a small amount of damage
        targetStats.AffectCurrentStamima(targetBlockStats.stamDecBlock, "dec");
        DealDamage(hittableObj, targetBlockStats.healthDecBlock);
    }

    // Coroutine so the attack can be delayed and dodged by the player
    protected IEnumerator UnblockableAttack()
    {
        if (Time.time >= nextUATime && doingUnblockable != true)
        {
            doingUnblockable = true; // To ensure the enemy can't use their unblockable attack twice

            flashScript.Flash(flashScript.GetFlashMaterial(1));
            enemyMovement.canMove = false;
            enemyMovement.StopMovement();
            
            yield return new WaitForSeconds(1);
             
            foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit().ToList()) // Deals the unblockable damage
            {
                DealDamage(hittableObj, enemyStats.uDmg);
            }
            Debug.Log("(E) Unblockable attack performed");
            
            enemyStats.AffectCurrentStamima(stamDecUAttack, "dec");
            
            nextUATime = Time.time + 1f / uARate;
            
            yield return new WaitForSeconds(1); // Waits for a second in game time to then allow the enemy to move again
            enemyMovement.canMove = true; 
            doingUnblockable = false;
        }
    }

    protected override void Throw()
    {
        if (Time.time >= nextThrowTime && doingUnblockable != true)
        {
            Debug.Log("(E) Throw attack performed");
            nextThrowTime = Time.time + 1f / throwRate;
        }
    }

    protected override void DefendLogic()
    {
        // Defend Feature
    }

    protected override void Block()
    {
        // Block Feature
    }

    protected override void Parry()
    {
        // Parry Feature
    }
}
