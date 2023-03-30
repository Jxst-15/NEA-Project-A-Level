using UnityEngine;

public class NimbleEMovement : EnemyMovement
{
    protected override void Movement()
    {
        direction = (targetPos.position - transform.position).normalized;

        if (enemyAI.fsm.currentState.thisStateID == EnemyStates.Tracking)
        {
            if (enemyAI.fsm.distanceFromTarget >= runDistance)
            {
                Run();
            }
            else if (enemyAI.fsm.distanceFromTarget < runDistance)
            {
                rb.velocity = new Vector2(direction.x * enemyStats.hSpeed, direction.y * enemyStats.vSpeed);
            }
        }
    }
}
