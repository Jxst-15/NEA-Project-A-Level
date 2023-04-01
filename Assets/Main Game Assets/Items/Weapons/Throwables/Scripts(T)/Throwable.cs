using UnityEngine;

public class Throwable : Weapon
{
    #region Fields
    #region Gameobjects
    public GameObject throwEnd;
    #endregion
    
    #region Script References
    public ThrowableUnique unique
    { get; private set; }
    #endregion

    #region Variables
    public bool isThrowing;
    private float throwSpeed;
    private float throwRange;
    #endregion

    private Rigidbody2D rb;


    private IComboMeter toUpdate;
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;

        unique = uniqueAttackBox.GetComponent<ThrowableUnique>();
    }
    #endregion

    protected override void SetVariables()
    {
        weaponType = WeaponType.Throwable;

        throwSpeed = 20f;
        throwRange = 5f;
        
        weaponLDmg = 20;
        weaponHDmg = 40;
        uniqueDmg = 55;

        stamDecWLAtk = 5;
        stamDecWHAtk = 10;
        stamDecWUEAtk = 15;

        hitsDone = 0;
        hitsToBreak = 6;
    }

    public override bool Attack(bool light)
    {
        bool objHit = base.Attack(light);
        // Code for attacking with this weapon
        return objHit;
    }

    public override bool UniqueAttack()
    {
        toUpdate = this.transform.parent.gameObject.transform.parent.gameObject.GetComponent<IComboMeter>();
        
        isThrowing = true;
        
        // Gets the parent of the hand object (Either the enemy or the player gameobject)
        objectHolding = this.transform.parent.gameObject.transform.parent.gameObject;

        objectHolding.GetComponent<IWeaponHandler>().SetWeaponToNull();
        objectHolding.GetComponent<IWeaponHandler>().SetWeaponHeld(false);

        this.transform.parent = null;

        this.hand = null;

        rb.isKinematic = false; // Needed in order to addforce to the object

        // Determine the direction
        Vector2 direction = ((throwEnd.transform.position) - transform.position).normalized;

        // Add force to the object
        rb.AddForce(direction * throwSpeed, ForceMode2D.Impulse); // Does some weird thing where sometimes it throws it upwards? Needs fix

        Destroy(gameObject, 2.5f);

        return true;
    }

    public void OnObjectHit()
    {
        toUpdate.ComboStart(1);
        BreakItem();
    }
}
