public class NormalEStats : EnemyStats
{
    protected override void SetVariables()
    {
        maxHealth = 300;
        attackRate = 2;
        lDmg = 30;
        hDmg = 50;
        uDmg = 70;

        maxStamina = 200;
        regenCooldown = 5;

        vSpeed = 2;
        hSpeed = 3;
        vRunSpeed = 4;
        hRunSpeed = 5;

        weakTo = "Iron Fist";

        toIncBy = 5;
        unstunCooldown = 7;

        base.SetVariables();
    }
}
