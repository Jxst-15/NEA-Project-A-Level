using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private List<Collider2D> enemiesHit = new List<Collider2D>();
    
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D enemy)
    {
        if (!enemiesHit.Contains(enemy) && enemy.gameObject.tag == "Enemies")
        {
            Debug.Log("Enemy entered trigger");
            enemiesHit.Add(enemy);
            Debug.Log("Enemy added to list");
        }
    }

    void OnTriggerExit2D(Collider2D enemy)
    {
        Debug.Log("Enemy exited trigger");
        if (enemiesHit.Contains(enemy))
        {
            enemiesHit.Remove(enemy);
            Debug.Log("Enemy removed from list");
        }
    }

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
