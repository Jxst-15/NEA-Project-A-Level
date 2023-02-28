public class Throwable : Weapon
{
    #region Variables
    private float throwSpeed;
    private float throwRange;
    #endregion

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void SetVariables()
    {
        weaponType = WeaponType.Throwable;

        throwSpeed = 2.5f;
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

    public override void UniqueAttack()
    {
        // Code for using the unique attack for this weapon
    }
}
