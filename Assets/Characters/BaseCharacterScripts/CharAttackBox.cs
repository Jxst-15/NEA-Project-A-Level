using System.Collections.Generic;
using UnityEngine;

public abstract class CharAttackBox : MonoBehaviour
{
    #region Variables
    // This is set in the child classes e.g. PlayerAttack would set toAttack to the layer index of the Enemy layer
    protected LayerMask toAttack;
    #endregion

    // Following list stores all colliders that have entered the gameobjects' box collider trigger
    protected List<Collider2D> objectsHit = new List<Collider2D>();

    // When a collider enters the attack collider trigger box
    protected void OnTriggerEnter2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == toAttack || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Add(hittableObj);
        }
    }

    // When a collider exits the attack collider trigger box
    protected void OnTriggerExit2D(Collider2D hittableObj)
    {
        if (objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == toAttack || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Remove(hittableObj);
        }
    }

    // Returns the list in order to deal damage
    public List<Collider2D> GetObjectsHit()
    {
        return objectsHit;
    }
}
