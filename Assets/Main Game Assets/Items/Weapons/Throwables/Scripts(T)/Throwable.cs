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
    #endregion

    private Rigidbody2D rb;
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
        
        weaponLDmg = 20;
        weaponHDmg = 35;
        uniqueDmg = 55;

        stamDecWLAtk = 5;
        stamDecWHAtk = 10;
        stamDecWUEAtk = 15;

        hitsDone = 0;
        hitsToBreak = 6;
    }

    public override bool UniqueAttack()
    {
        // Gets the parent of the hand object (Either the enemy or the player gameobject)
        objectHolding = this.transform.parent.gameObject.transform.parent.gameObject;
        
        if (objectHolding.TryGetComponent<IComboMeter>(out IComboMeter toUpdate))
        {
            toUpdate.ComboStart(1);
        }
        
        isThrowing = true;
        
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
        BreakItem();
    }
}
