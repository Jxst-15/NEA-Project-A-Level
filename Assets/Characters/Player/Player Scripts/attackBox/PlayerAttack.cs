using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Following list stores all enemy colliders within attack range
    private List<Collider2D> enemiesHit = new List<Collider2D>();

    // When a collider enters the player attack collider trigger box
    void OnTriggerEnter2D(Collider2D enemy)
    {
        if (!enemiesHit.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy entered trigger");
            enemiesHit.Add(enemy);
            Debug.Log("Enemy added to list");
        }
    }

    // When a collider exits the player attack collider trigger box
    void OnTriggerExit2D(Collider2D enemy)
    {
        if (enemiesHit.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy exited trigger");
            enemiesHit.Remove(enemy);
            Debug.Log("Enemy removed from list");
        }
    }

    // Returns the list in order to deal damage to players
    public List<Collider2D> GetEnemiesHit()
    {
        if (enemiesHit.Count == 0)
        {
            Debug.Log("List empty");
            return enemiesHit;
        }
        else
        {
            Debug.Log("List Returned");
            return enemiesHit;
        }
    }
}
