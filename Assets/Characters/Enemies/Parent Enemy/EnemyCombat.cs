using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour, ICharacterCombat
{
    #region Variables
    // Following is a weapon that has been picked up
    [SerializeField] private GameObject weapon;

    public GameObject attackBox, blockBox, parryBox;

    // LayerMasks help to identify which objects can be hit
    public LayerMask hittableObject;

    private float nextWAttackTime = 0f;
    private const float wAttackRate = 2f;

    private int randNum;
    [SerializeField] private float attackRange;
    [SerializeField] private int attackCount;
    private float nextAttackTime;

    // 0.5s, time which is needed for a parry to be valid
    private const float parryDelay = 0.5f;

    private float throwRate, nextThrowTime;

    // Variables for decreasing stat factors
    [SerializeField] private int stamDecLAttack, stamDecHAttack, stamDecUAttack, stamDecWUAttack, stamDecThrow, stamIncParry;
    #endregion

    #region Getters and Setters
    [SerializeField] private int stamDecBlock
    { get; set; }
    [SerializeField] private int healthDecBlock
    { get; set; }
    private bool canAttack
    { get; set; }
    private bool canDefend
    { get; set; }
    private bool attacking
    { get; set; }
    private bool blocking
    { get; set; }
    private bool throwing
    { get; set; }
    private float attackRate
    { get; set; }
    private bool weaponHeld
    { get; set; }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        canAttack = true;
        canDefend = true;
        blocking = false;

        attackRange = 1.5f;
        attackCount = 0;
        nextAttackTime = 0f;

        throwRate = 0.3f;
        nextThrowTime = 0f;

        attackRate = 2;

        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecUAttack = 30;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamDecBlock = 10;
        stamIncParry = 20;
        healthDecBlock = 5;
    }

    // Update is called once per frame
    void Update()
    {
        randNum = 0;
        randNum = Random.Range(1, 11);
        if (weapon != null && weapon.tag == "Weapons")
        {
            weaponHeld = true;
        }

        if (canAttack == true && canDefend == true)
        {
            if (1 <= randNum && randNum <= 7)
            {
                // Attack methods
            }
            else if (randNum == 8 || randNum == 9)
            {
                // Block method
            }
            else if (randNum == 10)
            {
                // Parry Method
            }
        }
    }

    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
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
            }
            else if (randNum == 10)
            {
                // Throw attack
                Throw();
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    public void UnblockableAttack()
    {
        Debug.Log("(E) Unblockable attack performed");
    }

    public void Throw()
    {
        if (Time.time >= nextThrowTime)
        {
            Debug.Log("(E) Throw attack performed");
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
