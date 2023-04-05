using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : BattleArea
{
    // Start is called before the first frame update
    protected override void Start()
    {
        areaCleared = false;
        enemies = new List<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
