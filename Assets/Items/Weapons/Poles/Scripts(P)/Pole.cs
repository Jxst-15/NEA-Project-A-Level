using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : Weapon
{
    #region Variables
    private float spinRange;
    #endregion

    //// Start is called before the first frame update
    //void Start()
    //{
    //    SetVariables();
    //}

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void SetVariables()
    {
        weaponType = WeaponType.Pole;

        spinRange = 2.5f;
        
        weaponLDmg = 60;
        weaponHDmg = 80;
        uniqueDmg = 95;

        hitsDone = 0;
        hitsToBreak = 20;
    }

    public override void Attack()
    {
        // Code for attacking with this weapon
    }

    public override void UniqueAttack()
    {
        // Code for using the unique attack for this weapon
    }
}
