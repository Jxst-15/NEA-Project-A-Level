using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBox : MonoBehaviour
{
    #region Fields
    #region Layermask
    // This is set in the child classes e.g. PlayerAttack would set toAttack to the layer index of the Enemy layer
    protected LayerMask toAttack;
    protected LayerMask canHit;
    #endregion
    
    // Following list stores all colliders that have entered the gameobjects' box collider trigger
    protected List<Collider2D> objectsHit = new List<Collider2D>();
    #endregion

    #region Unity Methods
    protected void Awake()
    {
        canHit = LayerMask.NameToLayer("CanHit");
    }
    #endregion

    #region OnTrigger Methods
    // When a collider enters the attack collider trigger box
    protected void OnTriggerEnter2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == toAttack || hittableObj.gameObject.layer == canHit)
        {
            objectsHit.Add(hittableObj);
        }
    }
    protected void OnTriggerStay2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == toAttack || hittableObj.gameObject.layer == canHit)
        {
            objectsHit.Add(hittableObj);
        }
    }

    // When a collider exits the attack collider trigger box
    protected void OnTriggerExit2D(Collider2D hittableObj)
    {
        if (objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == toAttack || hittableObj.gameObject.layer == canHit)
        {
            objectsHit.Remove(hittableObj);
        }
    }
    #endregion

    // Returns the list in order to deal damage
    public List<Collider2D> GetObjectsHit()
    {
        return objectsHit;
    }
}
