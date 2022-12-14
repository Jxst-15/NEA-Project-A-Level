using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Weapon : MonoBehaviour
{
    #region Variables
    protected string weaponName;
    protected float weaponRange;
    
    protected int weaponLDmg;
    protected int weaponHDmg;
    protected int weaponUDmg;
    protected int uniqueDmg;
    
    // How many times to attack in order to destroy the weapon
    protected int hitsToBreak;
    #endregion

    #region Getters and Setters
    public string getName() { return weaponName; }
    public void setName(string name) {  weaponName = name; }
    #endregion

    protected abstract void Attack();

    protected abstract void UniqueAttack();

    public void DropWeaapon()
    {
        // Code to drop weapon
    }
}
