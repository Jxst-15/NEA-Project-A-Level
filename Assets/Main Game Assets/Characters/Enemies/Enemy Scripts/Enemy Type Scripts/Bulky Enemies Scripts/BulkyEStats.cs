public class BulkyEStats : EnemyStats
{
    protected override void SetVariables()
    {
        maxHealth = 500;
        attackRate = 0.5f;
        lDmg = 50;
        hDmg = 70;
        uDmg = 90;

        maxStamina = 100;
        regenCooldown = 7;

        vSpeed = 1;
        hSpeed = 2;
        vRunSpeed = 3;
        hRunSpeed = 4;

        weakTo = "Boulder Style";

        toIncBy = 5;
        unstunCooldown = 7;

        base.SetVariables();
    }
}
