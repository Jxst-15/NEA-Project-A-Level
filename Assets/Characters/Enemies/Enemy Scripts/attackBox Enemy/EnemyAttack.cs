using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private List<Collider2D> objectsHit = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D player)
    {
        //if (!objectsHit.Contains(player) && player.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    objectsHit.Add(player);
        //}
        Debug.Log("Player entered attack box");
    }

    void OnTriggerExit2D(Collider2D player)
    {
        //if (objectsHit.Contains(player) && player.gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    objectsHit.Remove(player);
        //}
        Debug.Log("Player exited attack box");
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
