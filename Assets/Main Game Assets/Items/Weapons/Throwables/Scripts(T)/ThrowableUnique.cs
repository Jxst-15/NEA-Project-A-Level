using UnityEngine;

public class ThrowableUnique : MonoBehaviour // Could have it inherit from attack box?
{
    #region Fields
    #region Script References
    public Throwable thisWeapon;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Awake()
    {
        thisWeapon = this.GetComponentInParent<Throwable>();
    }
    #endregion

    public void ThrowingWeapon(Collider2D collision)
    {
        if (thisWeapon.isThrowing)
        {
            if (collision.gameObject.layer == thisWeapon.uniqueWeaponAttack.GetToAttack() || collision.gameObject.layer == thisWeapon.uniqueWeaponAttack.GetCanHit())
            {
                GameObject toDamage = collision.gameObject;

                // Deal damage to the first gameobject it comes in contact with
                toDamage.GetComponent<IDamageable>().TakeDamage(thisWeapon.uniqueDmg, true);
                thisWeapon.OnObjectHit();
            }
        }
    }

    #region OnTriggerMethods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThrowingWeapon(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ThrowingWeapon(collision);
    }
    #endregion
}
