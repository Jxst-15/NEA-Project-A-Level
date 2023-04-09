public class NimbleEStats : EnemyStats
{
    protected override void SetVariables()
    {
        maxHealth = 200;
        attackRate = 3;
        lDmg = 10;
        hDmg = 20;
        uDmg = 40;

        maxStamina = 300;
        regenCooldown = 3;

        vSpeed = 3;
        hSpeed = 4;
        vRunSpeed = 5;
        hRunSpeed = 6;

        weakTo = "Grass Style";

        toIncBy = 5;
        unstunCooldown = 7;

        base.SetVariables();
    }
}
