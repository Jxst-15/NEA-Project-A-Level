using UnityEngine;

public class Throwable : Weapon
{
    #region Fields
    #region Variables
    public bool isThrowing;
    private float throwSpeed;
    private float throwRange;
    #endregion

    private Rigidbody2D rb;

    public GameObject throwEnd;
    #endregion

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
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
        bool objHit = false;
        
        if (Time.time >= nextWUAttackTime)
        {
            isThrowing = true;
            // Gets the parent of the hand object (Either the enemy or the player gameobject)
            // this.transform.parent.gameObject.transform.parent.GetComponent<IWeaponHandler>().SetWeaponHeld(false);
            this.transform.parent.gameObject.transform.parent.GetComponent<IWeaponHandler>().SetWeaponToNull();
            this.transform.parent = null;
            this.hand = null;
            rb.isKinematic = false;
            // Determine the direction
            Vector2 direction = ((throwEnd.transform.position) - transform.position).normalized;
            // Add force to the object
            rb.AddForce(direction * throwSpeed, ForceMode2D.Impulse); // Does some weird thing where sometimes it throws it upwards? Needs fix
            
            nextWUAttackTime = Time.time + 1f / wUAttackRate;
        }
        return objHit;
    }

    public void OnObjectHit()
    {
        BreakItem();
    }
}
