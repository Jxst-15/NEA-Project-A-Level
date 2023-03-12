using UnityEngine;

public class ThrowableUnique : MonoBehaviour // Could have it inherit from attack box?
{
    #region Script References
    public Throwable thisWeapon;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        thisWeapon = this.GetComponentInParent<Throwable>();
    }

    private void ThrowingWeapon(Collider2D collision)
    {
        if (thisWeapon.isThrowing)
        {
            if (collision.gameObject.layer == thisWeapon.uniqueWeaponAttack.GetToAttack() || collision.gameObject.layer == thisWeapon.uniqueWeaponAttack.GetCanHit())
            {
                GameObject toDamage = collision.gameObject;

                // Deal damage to the first gameobject it comes in contact with
                toDamage.GetComponent<IDamageable>().TakeDamage(thisWeapon.uniqueDmg);
                thisWeapon.OnObjectHit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThrowingWeapon(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ThrowingWeapon(collision);
    }
}
