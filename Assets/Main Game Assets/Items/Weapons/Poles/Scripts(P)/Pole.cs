public class Pole : Weapon
{
    protected override void SetVariables()
    {
        weaponType = WeaponType.Pole;
        
        weaponLDmg = 60;
        weaponHDmg = 80;
        uniqueDmg = 95;

        stamDecWLAtk = 15;
        stamDecWHAtk = 20;
        stamDecWUEAtk = 30;

        hitsDone = 0;
        hitsToBreak = 20;
    }

    public override bool Attack(bool light)
    {
        bool objHit =  base.Attack(light);
        // Code for attacking with this weapon
        return objHit;
    }
}
