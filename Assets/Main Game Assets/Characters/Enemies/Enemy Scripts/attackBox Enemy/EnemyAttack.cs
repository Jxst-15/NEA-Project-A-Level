using UnityEngine;

public class EnemyAttack : AttackBox
{
    private void Start()
    {
        toAttack = LayerMask.NameToLayer("Player");
    }
}
