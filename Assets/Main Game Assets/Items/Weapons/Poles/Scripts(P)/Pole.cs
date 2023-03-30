public class Pole : Weapon
{
    protected override void SetVariables()
    {
        weaponType = WeaponType.Pole;
        
        weaponLDmg = 60;
        weaponHDmg = 80;
        uniqueDmg = 95;

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
