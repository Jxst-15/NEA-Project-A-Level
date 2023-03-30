using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    #region Fields
    #region Getters and Setters
    public bool areaCleared
    { get; set; }

    public int eToDefeat
    { get; set; }

    public int waves
    { get; private set; }
    
    public List<Collider2D> enemies 
    { get; private set; }
    #endregion

    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        areaCleared = false;
        System.Random rand = new System.Random();
        
        eToDefeat = rand.Next(10 / 2, 14 / 2) * 2;

        waves = rand.Next(2, 4);

        enemies = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.gameObject.layer == LayerMask.NameToLayer("Enemy") && !enemies.Contains(entity) && entity.gameObject.name != "groundBox")
        {
            enemies.Add(entity);
        }
    }

    private void OnTriggerExit2D(Collider2D entity)
    {
        if (enemies.Contains(entity))
        {
            enemies.Remove(entity);
        }
    }
    #endregion
}
