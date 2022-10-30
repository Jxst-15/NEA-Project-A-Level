using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Following list stores all enemy colliders within attack range
    private List<Collider2D> objectsHit = new List<Collider2D>();

    // When a collider enters the player attack collider trigger box
    void OnTriggerEnter2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Enemy") || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit")) 
        {
            objectsHit.Add(hittableObj);
            // Debug.Log("Object added to list (P)");
        }
    }

    // When a collider exits the player attack collider trigger box
    void OnTriggerExit2D(Collider2D hittableObj)
    {
        if (objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Enemy") || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Remove(hittableObj);
            // Debug.Log("Object removed from list (P)");
        }
    }

    // Returns the list in order to deal damage to players
    public List<Collider2D> GetObjectsHit()
    {
        // Debug.Log("List returned "+ objectsHit.Count);
        return objectsHit;
    }
}
