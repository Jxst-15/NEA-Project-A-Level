public class BladedWeapon : Weapon
{
    protected override void SetVariables()
    {
        weaponType = WeaponType.BladedWeapon;

        weaponLDmg = 70;
        weaponHDmg = 90;
        uniqueDmg = 105;

        stamDecWLAtk = 15;
        stamDecWHAtk = 25;
        stamDecWUEAtk = 35;

        hitsDone = 0;
        hitsToBreak = 15;
    }
    
    public override bool Attack(bool light)
    {
        bool objHit = base.Attack(light);
        return objHit;
    }
}
