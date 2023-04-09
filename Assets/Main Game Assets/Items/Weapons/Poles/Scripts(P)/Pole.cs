public class Pole : Weapon
{
    protected override void SetVariables()
    {
        weaponType = WeaponType.Pole;
        
        weaponLDmg = 45;
        weaponHDmg = 65;
        uniqueDmg = 80;

        stamDecWLAtk = 15;
        stamDecWHAtk = 20;
        stamDecWUEAtk = 30;

        hitsDone = 0;
        hitsToBreak = 20;
    }
}
