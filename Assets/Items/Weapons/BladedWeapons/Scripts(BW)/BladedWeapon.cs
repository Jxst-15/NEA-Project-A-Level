public class BladedWeapon : Weapon
{
    #region Fields
    #region Variables
    private float stabRange;
    private float stabTime;
    #endregion
    #endregion

    protected override void SetVariables()
    {
        weaponType = WeaponType.BladedWeapon;

        stabRange = 2.5f;
        weaponLDmg = 70;
        weaponHDmg = 90;
        uniqueDmg = 105;

        hitsDone = 0;
        hitsToBreak = 15;
    }
    
    public override bool Attack(bool light)
    {
        bool objHit = base.Attack(light);
        // Code for attacking with this weapon
        return objHit;
    }
}
