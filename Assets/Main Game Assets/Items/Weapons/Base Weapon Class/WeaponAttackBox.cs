using UnityEngine;

public class WeaponAttackBox : AttackBox
{
    // Activated when the GameObject is set to active
    void OnEnable()
    {
        // If the hand assigned to the weapon is not empty
        if (GetComponentInParent<Weapon>().hand != null)
        {
            if (GetComponentInParent<Weapon>().hand.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // If the hand belongs to the player the toAttack layer is set to Enemy
                toAttack = LayerMask.NameToLayer("Enemy");
            }
            else if (GetComponentInParent<Weapon>().hand.transform.parent.gameObject.layer == LayerMask.NameToLayer("Enemy")) // If it is an enemy holding
            {
                toAttack = LayerMask.NameToLayer("Player");
                Debug.Log("Active");
            }
        }
    }

    // Runs when the GameObject is disabled
    void OnDisable()
    {
        toAttack = LayerMask.NameToLayer("Blank");
    }

    public LayerMask GetToAttack()
    {
        return toAttack;
    }

    public LayerMask GetCanHit()
    {
        return canHit;
    }
}
