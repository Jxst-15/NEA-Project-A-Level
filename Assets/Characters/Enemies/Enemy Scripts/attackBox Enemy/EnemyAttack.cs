using UnityEngine;

public class EnemyAttack : CharAttackBox
{
    #region W/O Inheritance
    /*
    private List<Collider2D> objectsHit = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D hittableObj)
    {
        if (!objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Player") || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Add(hittableObj);
        }
    }

    void OnTriggerExit2D(Collider2D hittableObj)
    {
        if (objectsHit.Contains(hittableObj) && hittableObj.gameObject.layer == LayerMask.NameToLayer("Player") || hittableObj.gameObject.layer == LayerMask.NameToLayer("CanHit"))
        {
            objectsHit.Remove(hittableObj);
        }
    }

    public List<Collider2D> GetObjectsHit()
    {
        // Debug.Log("List returned "+ objectsHit.Count);
        return objectsHit;
    }
    */
    #endregion

    private void Start()
    {
        toAttack = LayerMask.NameToLayer("Player");
    }
}
