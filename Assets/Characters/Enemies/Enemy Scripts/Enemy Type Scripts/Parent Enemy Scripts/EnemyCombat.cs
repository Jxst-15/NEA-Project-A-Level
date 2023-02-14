using UnityEngine;

public class EnemyCombat : MonoBehaviour, ICharacterCombat
{
    #region Script References
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private EnemyStats enemyStats;

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

    private int randNum;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackCount;
    private float nextAttackTime;

    private float uaRate, nextUATime;

    private float throwRate, nextThrowTime;

    // Variables for decreasing stat factors
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
    #endregion

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
        enemyAI = GetComponent<EnemyAI>();
        enemyStats = GetComponent<EnemyStats>();

        //targetBlockStats = target.GetComponentInChildren<PlayerBlock>();
        //targetAttackStatus = target.GetComponent<PlayerCombat>();
        //targetStats = target.GetComponent<PlayerStats>();
        
        
        attackBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.isPaused == false)
        {
            ECombatUpdate();
        }
    }

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
        attackRate = 2;
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

    public void ECombatUpdate()
    {
        switch (enemyAI.inRange)
        {
            case true:
                if (weapon == null)
                {
                    attackBox.SetActive(true);
                    ProbabilityOfActions();
                }
                else
                {
                    weapon.GetComponent<Weapon>().Attack();
                }
            break;
            default:
                attackBox.SetActive(false);
            break;
        }
    }

    public void ProbabilityOfActions()
    {
        // To determine which action to do
        randNum = 0;
        randNum = Random.Range(1, 11);
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
        int dmgToDeal = 0;
        if (Time.time >= nextAttackTime)
        {
            attacking = true;
            // The probability of attacking
            randNum = Random.Range(1, 11); 
            
            if (targetAttackStatus.blocking != true)
            {
                if (1 <= randNum && randNum <= 8)
                {
                    // Normal attacks
                    foreach (Collider2D hittableobj in enemyAttack.GetObjectsHit())
                    {

                        if (1 <= randNum && randNum <= 6)
                        {
                            // Light attack
                            dmgToDeal = enemyStats.lDmg;
                            Debug.Log("Player hit (L)");
                        }
                        else if (randNum == 7 || randNum == 8)
                        {
                            // Heavy attack
                            dmgToDeal = enemyStats.hDmg;
                            Debug.Log("Player hit (H)");
                        }
                        DealDamage(hittableobj, dmgToDeal);
                    }
                }
            }
            else
            {
                Debug.Log("Attack was blocked!");
                
                // The following decreases the targets current stamina and deals a small amount of damage
                targetStats.AffectCurrentStamima(targetBlockStats.stamDecBlock, "dec");
                targetStats.TakeDamage(targetBlockStats.healthDecBlock);

            }

            if (randNum == 9)
            {
                // Unblockable attack
                UnblockableAttack();
            }
            else if (randNum == 10)
            {
                // Throw attack
                Throw();
            }
            nextAttackTime = Time.time + 1f / attackRate;
            // Debug.Log("Next attack time is: " + nextAttackTime);
        }
    }

    public void UnblockableAttack()
    {
        if (Time.time >= nextUATime)
        {
            // Debug.Log("(E) Unblockable attack performed");
            foreach (Collider2D hittableObj in enemyAttack.GetObjectsHit())
            {
                DealDamage(hittableObj, enemyStats.uDmg);
                Debug.Log("Player hit (U), next U time is: "+ nextUATime);
            }
            nextUATime = Time.time + 1f / uaRate;
        }
    }

    public void DealDamage(Collider2D hittableObj, int dmgToDeal)
    {
        hittableObj.GetComponent<IDamageable>().TakeDamage(dmgToDeal);
    }

    public void Throw()
    {
        if (Time.time >= nextThrowTime)
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
