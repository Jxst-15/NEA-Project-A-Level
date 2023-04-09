using UnityEngine;

// USed for gameobjects using weapons
public abstract class WeaponCombatHandler : MonoBehaviour, IWeaponHandler
{
    #region Fields
    #region Gameobjects
    // The actual weapon object
    public GameObject weapon
    { get; set; }
    #endregion

    #region Script References
    public Weapon weaponScript
    { get; set; }
    #endregion

    #region Getters and Setters
    public bool weaponHeld
    { get; set; }
    #endregion
    #endregion

    public abstract void WeaponAttackLogic();

    public void WeaponAttack(bool lightAtk, bool unique, int toDecBy)
    {
        if (!unique)
        {
            if (weaponScript.Attack(lightAtk) == true)
            {
                if (weapon != null)
                {
                    UseNormalWeaponAtk(toDecBy);
                }
            }
        }
        else
        {
            if (weaponScript.UniqueAttack() == true)
            {
                if (weapon != null)
                {
                    UseUniqueWeaponAtk(toDecBy);
                }
            }
        }
    }

    protected abstract void UseNormalWeaponAtk(int toDecBy);

    protected abstract void UseUniqueWeaponAtk(int toDecBy);

    public void SetWeaponToNull()
    {
        weapon = null;
        Debug.Log("Weapon set to null");
    }

    public void SetWeaponHeld(bool val)
    {
        weaponHeld = val;
        Debug.Log("Weapon held is false");
    }
}
