using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : EnemyScript, ICharacterCombat
{
    #region Script References
    protected PlayerCombat targetAttackStatus;
    protected PlayerStats targetStats;
    #endregion

    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] private GameObject weapon;

    public GameObject attackBox, blockBox, parryBox;

    // LayerMasks help to identify which objects can be hit
    public LayerMask hittableObject;

    public EnemyAttack enemyAttack;
    
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
    // Start is called before the first frame update
    void Start()
    {
        enemyAttack = attackBox.GetComponent<EnemyAttack>();

        attackBox.SetActive(false);

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

        targetAttackStatus = target.GetComponent<PlayerCombat>();
        targetStats = target.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAI.inRange == true)
        {
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
                    // Attack methods
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
    }

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            attackBox.SetActive(true);
            attacking = true;
            randNum = Random.Range(1, 11);
            if (1 <= randNum && randNum <= 6)
            {
                // Light attack
                Debug.Log("(E) Light attack performed");     
            }
            else if (randNum == 7 || randNum == 8)
            {
                // Heavy attack
                Debug.Log("(E) Heavy attack performed");
            }
            else if (randNum == 9)
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
            attackBox.SetActive(false);
        }
    }

    public void UnblockableAttack()
    {
        if (Time.time >= nextUATime)
        {
            Debug.Log("(E) Unblockable attack performed");
            nextUATime = Time.time + 1f / uaRate;
        }
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

    public void WeaponAtack()
    {

    }
}
