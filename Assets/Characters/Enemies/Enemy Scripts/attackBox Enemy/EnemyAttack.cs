using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private List<Collider2D> objectsHit = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            objectsHit.Add(hittableObj);
            // Debug.Log("Object removed from list (E)");
        }
    }

    void OnTriggerExit2D(Collider2D hittableObj)
    {
        if (objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Player") || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Remove(hittableObj);
            // Debug.Log("Object removed from list (E)");
        }
    }

    public List<Collider2D> GetObjectsHit()
    {
        // Debug.Log("List returned "+ objectsHit.Count);
        return objectsHit;
    }
}
