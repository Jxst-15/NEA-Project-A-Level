using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Throwable,
        Pole,
        BladedWeapon
    }

    #region Variables
    protected WeaponType weaponType;
    protected string weaponName;
    protected float weaponRange;
    
    protected int weaponLDmg;
    protected int weaponHDmg;
    protected int weaponUDmg;
    protected int uniqueDmg;

    // How many times to attack in order to destroy the weapon
    protected int hitsDone;
    protected int hitsToBreak;
    #endregion

    #region Getters and Setters
    public string getName() { return weaponName; }
    public void setName(string name) {  weaponName = name; }
    public int getHitsDone() { return hitsDone; }
    #endregion

    protected virtual void Start()
    {
        SetVariables();
    }

    protected virtual void Update()
    {
        CheckIfBroken();
    }

    protected abstract void SetVariables();

    public abstract void Attack();

    public abstract void UniqueAttack();

    public void DropItem()
    {
        // Code to drop weapon
    }

    public void UpdateHitsDone()
    {
        hitsDone++;
        if (CheckIfBroken() == true)
        {
            BreakItem();
        }
    }

    protected bool CheckIfBroken()
    {
        bool broken = false;
        if (hitsDone == hitsToBreak)
        {
            broken = true;
        }
        return broken;
    }

    protected void BreakItem()
    {
        Debug.Log("Weapon was destroyed!");
        Destroy(gameObject);
    }
}
