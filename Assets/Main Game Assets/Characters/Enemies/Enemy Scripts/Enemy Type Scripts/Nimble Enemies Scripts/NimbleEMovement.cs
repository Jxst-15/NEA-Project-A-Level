using UnityEngine;

public class NimbleEMovement : EnemyMovement
{
    // This method has no StopMoving, this makes it so the nimble enemy continuously moves
    protected override void Movement()
    {
        Vector2 direction = (targetPos.position - transform.position).normalized;

        if (enemyAI.fsm.currentState.thisStateID == EnemyStates.Tracking)
        {
            if (enemyAI.fsm.distanceFromTarget >= runDistance)
            {
                Run(direction);
            }
            else if (enemyAI.fsm.distanceFromTarget < runDistance)
            {
                // Normally walk
                rb.velocity = new Vector2(direction.x * enemyStats.hSpeed, direction.y * enemyStats.vSpeed);
            }
        }
    }
}
