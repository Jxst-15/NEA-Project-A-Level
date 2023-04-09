public class BladedWeapon : Weapon
{
    protected override void SetVariables()
    {
        weaponType = WeaponType.BladedWeapon;

        weaponLDmg = 60;
        weaponHDmg = 80;
        uniqueDmg = 95;

        stamDecWLAtk = 15;
        stamDecWHAtk = 25;
        stamDecWUEAtk = 35;

        hitsDone = 0;
        hitsToBreak = 15;
    }
}
