using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyCombat : CharCombat, IWeaponHandler
{
    #region Fields
    #region Object References
    private System.Random rng = new System.Random();
    #endregion

    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private FlashScript flashScript;

    [SerializeField] private PlayerCombat targetAttackStatus;
    [SerializeField] private PlayerBlock targetBlockStats;
    [SerializeField] private PlayerStats targetStats;
    #endregion

    #region Target
    private GameObject target;
    #endregion
    
    #region Variables
    private float uaRate, nextUATime;
    private bool doingUnblockable;

    // Variables for affecting stat factors
    [SerializeField] private int stamDecUAttack;
    #endregion

    #region Getters and Setters
    [SerializeField] public int stamDecBlock
    { get; set; }
    [SerializeField] public int healthDecBlock
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

        enemyAttack = attackBox.GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();
        enemyStats = GetComponent<EnemyStats>();
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
        if (PauseMenu.isPaused == false || canAttack == false)
        {
            switch (enemyAI.fsm.currentState.thisStateID == EnemyStates.Attacking)
            {
                case true:
                    if (weapon == null)
                    {
                        attackBox.SetActive(true);
                        ProbabilityOfActions();
                    }
                    else
                    {
                        weapon.GetComponent<Weapon>().Attack(lightAtk); // WIP
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
        uaRate = 0.1f;
        nextUATime = 0f;

        throwRate = 0.3f;
        nextThrowTime = 0f;

        stamDecUAttack = 30;
        stamDecBlock = 10;
        healthDecBlock = 5;

        base.SetVariables();
    }

    protected virtual void ProbabilityOfActions()
    {
        // To determine which action to do
        int randNum = rng.Next(1, 11);
        if (weapon != null && weapon.tag == "Weapons")
        {
            weaponHeld = true;
        }

        if (canAttack == true && canDefend == true && weaponHeld == false && enemyStats.stun != true)
        {
            if (1 <= randNum && randNum <= 7)
            {
                // Attack method
                Attack();
            }
            else if (randNum == 8 || randNum == 9 && targetAttackStatus.attacking == true)
            {
                // Block method
                Block();
            }
            else if (randNum == 10 && targetAttackStatus.parryable == true)
            {
                // Parry Method
                Parry();
            }
        }
    }

    protected override void AttackLogic()
    {
        throw new System.NotImplementedException();
    }

    protected override void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            attacking = true;
            if (doingUnblockable != true)
            {
                // The probability of attacking
                int randNum = rng.Next(1, 11);

                if (1 <= randNum && randNum <= 8)
                {
                    int dmgToDeal = 0;
                    foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit().ToList())
                    {
                        if (hittableObj != null)
                        {
                            switch (gettingBlocked)
                            {
                                case false:
                                    switch (randNum)
                                    {
                                        case int i when i >= 1 && i <= 6:
                                            lightAtk = true;
                                            dmgToDeal = enemyStats.lDmg;
                                            break;
                                        case int i when i == 7 || i == 8:
                                            lightAtk = false;
                                            dmgToDeal = enemyStats.hDmg;
                                            break;
                                    }
                                    DealDamage(hittableObj, dmgToDeal);
                                    break;
                                case true:
                                    AttackWhenBlocking(hittableObj);
                                    break;
                            }
                        }
                    }
                }

                // These attacks can still be done even if the player is blocking
                if (randNum == 9)
                {
                    // Unblockable attack
                    StartCoroutine(UnblockableAttack());
                }
                else if (randNum == 10)
                {
                    Throw();
                }
                lightAtk = false;
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    protected override void AttackWhenBlocking(Collider2D hittableObj)
    {
        // The following decreases the targets current stamina and deals a small amount of damage
        targetStats.AffectCurrentStamima(targetBlockStats.stamDecBlock, "dec");
        DealDamage(hittableObj, targetBlockStats.healthDecBlock);
    }

    // Coroutine so the attack can be delayed and dodged by the player
    private IEnumerator UnblockableAttack()
    {
        if (Time.time >= nextUATime && doingUnblockable != true)
        {
            doingUnblockable = true; // To ensure the enemy can't use their unblockable attack twice

            flashScript.Flash(flashScript.GetFlashMaterial(1));
            enemyMovement.canMove = false;
            enemyMovement.StopMovement();
            
            yield return new WaitForSeconds(1);
             
            foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit().ToList())
            {
                DealDamage(hittableObj, enemyStats.uDmg);
            }
            Debug.Log("(E) Unblockable attack performed");           
            
            
            nextUATime = Time.time + 1f / uaRate;
            
            yield return new WaitForSeconds(1);
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
        throw new System.NotImplementedException();
    }

    protected override void Block()
    {

    }

    protected override void Parry()
    {

    }

    public void SetWeaponToNull()
    {
        throw new System.NotImplementedException();
    }

    public void SetWeaponHeld(bool val)
    {
        weaponHeld = val;
    }
}
