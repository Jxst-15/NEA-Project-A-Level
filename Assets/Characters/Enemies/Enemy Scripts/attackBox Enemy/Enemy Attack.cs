using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyCombat
{
    private List<Collider2D> objectsHit = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D player)
    {
        //if (!objectsHit.Contains(player) && player.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    objectsHit.Add(player);
        //}
    }

    void OnTriggerExit2D(Collider2D player)
    {
        //if (objectsHit.Contains(player) && player.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    objectsHit.Remove(player);
        //}
    }

    //public List<Collider2D> GetObjectsHit()
    //{
    //    if (objectsHit.Count == 0)
    //    {
    //        return objectsHit;
    //    }
    //    else
    //    { 
    //        return objectsHit;
    //    }
    //}
}
