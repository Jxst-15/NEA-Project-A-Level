using System.Collections;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, ICharacterCombat
{
    #region Fields
    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyStats enemyStats;
    [SerializeField] private FlashScript flashScript;

    // These are set in the inspector within unity
    [SerializeField] private PlayerCombat targetAttackStatus;
    [SerializeField] private PlayerBlock targetBlockStats;
    [SerializeField] private PlayerStats targetStats;
    #endregion

    #region Target
    private GameObject target;
    #endregion
    
    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] private GameObject weapon;

    public GameObject attackBox, blockBox, parryBox;

    // LayerMasks help to identify which objects can be hit
    public LayerMask player, hittableObject;

    // 0.5s, time which is needed for a parry to be valid
    private const float parryDelay = 0.5f;

    private float nextWAttackTime = 0f;
    private const float wAttackRate = 2f;

    private bool lightAtk;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackCount;
    private float nextAttackTime;

    private float uaRate, nextUATime;
    private bool doingUnblockable;

    private float throwRate, nextThrowTime;

    // Variables for affecting stat factors
    [SerializeField] private int stamDecLAttack, stamDecHAttack, stamDecUAttack, stamDecWUAttack, stamDecThrow, stamIncParry;
    #endregion

    #region Getters and Setters
    [SerializeField] public int stamDecBlock
    { get; set; }
    [SerializeField] public int healthDecBlock
    { get; set; }
    public bool canAttack
    { get; set; }
    public bool canDefend
    { get; set; }
    public bool attacking
    { get; set; }
    public bool blocking
    { get; set; }
    public bool throwing
    { get; set; }
    public bool parryable
    {  get; set; }
    public float attackRate
    { get; set; }
    public bool weaponHeld
    { get; set; }
    public bool gettingBlocked
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    void Awake()
    {
        SetVariables();      
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        target = enemyScript.target;

        enemyAttack = attackBox.GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyAI = GetComponent<EnemyAI>();
        enemyStats = GetComponent<EnemyStats>();
        flashScript = GetComponent<FlashScript>();

        attackRate = enemyStats.attackRate;

        attackBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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

    private void SetVariables()
    {
        canAttack = true;
        canDefend = true;
        attacking = false;
        blocking = false;
        throwing = false;
        parryable = false;

        attackRange = 1.5f;
        attackCount = 0;
        nextAttackTime = 0f;

        uaRate = 0.1f;
        nextUATime = 0f;

        throwRate = 0.3f;
        nextThrowTime = 0f;


        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecUAttack = 30;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamDecBlock = 10;
        stamIncParry = 20;
        healthDecBlock = 5;
    }

    public void ProbabilityOfActions()
    {
        // To determine which action to do
        // randNum = 0;
        int randNum = Random.Range(1, 11);
        if (weapon != null && weapon.tag == "Weapons")
        {
            weaponHeld = true;
        }

        if (canAttack == true && canDefend == true && weaponHeld == false)
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

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            attacking = true;
            if (doingUnblockable != true)
            {
                // The probability of attacking
                int randNum = Random.Range(1, 11);

                if (enemyStats.stun != true)
                {
                    if (1 <= randNum && randNum <= 8)
                    {
                        int dmgToDeal = 0;
                        foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit())
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
    }

    public void AttackWhenBlocking(Collider2D hittableObj)
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
            doingUnblockable = true;
            flashScript.Flash(flashScript.GetFlashMaterial(1));
            enemyMovement.canMove = false;
            
            yield return new WaitForSeconds(1);
            
            enemyMovement.canMove = true; 
            
            foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit())
            {
                DealDamage(hittableObj, enemyStats.uDmg);
            }
            Debug.Log("(E) Unblockable attack performed");           
            
            nextUATime = Time.time + 1f / uaRate;
            
            yield return new WaitForSeconds(1);
            doingUnblockable = false;
        }
    }

    public void DealDamage(Collider2D hittableObj, int dmgToDeal)
    {
        hittableObj.GetComponent<IDamageable>().TakeDamage(dmgToDeal);
    }

    public void Throw()
    {
        if (Time.time >= nextThrowTime && doingUnblockable != true)
        {
            Debug.Log("(E) Throw attack performed");
            nextThrowTime = Time.time + 1f / throwRate;
        }
    }

    public void Block()
    {

    }

    public void Parry()
    {

    }
}
