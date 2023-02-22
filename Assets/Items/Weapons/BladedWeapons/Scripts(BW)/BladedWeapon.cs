public class BladedWeapon : Weapon
{
    #region Variables
    private float stabRange;
    private float stabTime;
    #endregion

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
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
    
    public override void Attack()
    {
        base.Attack();
        // Code for attacking with this weapon
    }

    public override void UniqueAttack()
    {
        // Code for using the unique attack for this weapon
    }

}
