using UnityEngine;

public class BossEStats : EnemyStats
{
    protected override void SetVariables()
    {
        maxHealth = 1050;
        attackRate = 1f;
        lDmg = 80;
        hDmg = 100;
        uDmg = 120;

        maxStamina = 400;
        regenCooldown = 3;

        vSpeed = 2;
        hSpeed = 3;
        vRunSpeed = 4;
        hRunSpeed = 5;

        weakTo = "None"; // Boss Enemy has no weakness

        toIncBy = 15;
        unstunCooldown = 10;

        base.SetVariables();
    }

    protected override void Death()
    {
        // As the enemy has no health left, switch states to Inactive
        enemyAI.fsm.MoveStates(EnemyCommands.NoHealth);

        // On death, gives the player points and increases their number of enemies defeated
        enemyScript.OnDeath();

        flashScript.Flash(flashScript.GetFlashMaterial(2));

        GameObject.Find("UI Canvas").GetComponent<FinishMenu>().finishedGame = true;

        // Destroying the game object helps to manage memory and declutter screen
        Destroy(gameObject);
    }
}
