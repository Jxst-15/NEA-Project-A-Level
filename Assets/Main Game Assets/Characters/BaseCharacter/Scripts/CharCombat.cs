using UnityEngine;

public abstract class CharCombat : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject attackBox, blockBox, parryBox, throwBox;

    // Following is a weapon that has been picked up
    [SerializeField] public GameObject weapon;
    #endregion

    #region Variables
    [SerializeField] protected int attackCount;
    protected float nextAttackTime;
    protected bool lightAtk;


    protected float throwRate, nextThrowTime;
    #endregion

    // 0.5s, time which is needed for a parry to be valid
    protected const float parryDelay = 0.5f;

    #region Getters and Setters
    public float attackRate
    { get; set; }
    
    // Variables for decreasing/increasing stat factors
    public int stamDecLAttack
    { get; set; }
    public int stamDecHAttack
    { get; set; }
    public int stamDecWUAttack
    { get; set; }
    public int stamDecThrow
    { get; set; }
    public int stamIncParry
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
    { get; set; }
   
    public bool weaponHeld
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        SetVariables();
    }

    // Update is called once per frame
    protected abstract void Update();
    #endregion

    protected virtual void SetVariables()
    {
        canAttack = true;
        canDefend = true;
        attacking = false;
        blocking = false;
        throwing = false;
        parryable = false;

        nextAttackTime = 0f;

        lightAtk = false;

        weaponHeld = false;

        stamDecLAttack = 15;
        stamDecHAttack = 20;
        stamDecWUAttack = 30;
        stamDecThrow = 35;
        stamIncParry = 20;
    }

    protected abstract void AttackLogic();

    protected abstract void DefendLogic();

    protected abstract void Attack();

    protected abstract void AttackWhenBlocking(Collider2D hittableObj);

    protected void DealDamage(Collider2D hittableObj, int dmgToDeal)
    {
        hittableObj.GetComponent<IDamageable>().TakeDamage(dmgToDeal, false);
    }

    protected abstract void Throw();

    protected abstract void Block();

    protected abstract void Parry();
}
