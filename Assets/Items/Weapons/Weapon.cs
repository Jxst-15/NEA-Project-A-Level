using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an abstract class meaning that this cannot be instantiated therefore it only serves the purpose of 
// a way for the other weapons to inherit what methods this has in order to save copying code
public abstract class Weapon : MonoBehaviour, IInteractable
{
    // enum for all the weapon types within the game
    public enum WeaponType
    {
        Throwable,
        Pole,
        BladedWeapon
    }

    #region GameObjects
    public GameObject hand;
    #endregion

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

    // I have made these methods virtual so they can be overriden in the actual weapon scripts
    protected virtual void Start()
    {
        SetVariables();
    }

    protected virtual void Update()
    {
        CheckIfBroken();
    }

    protected abstract void SetVariables();

    public void Interact()
    {
        Debug.Log("Weapon was picked up!");
        
        // Set the position of the weapon to where the hand GameObject is and set its parent to the parent that the hand is attached to
        this.transform.position = new Vector2(hand.transform.position.x, hand.transform.position.y);
        this.transform.parent = hand.transform;
        // Destroy(gameObject);
    }

    public virtual void Attack()
    {
        Debug.Log("Attacked with weapon!");
    }

    public abstract void UniqueAttack();

    public void DropItem()
    {
        // Code to drop weapon
        Debug.Log("Weapon was dropped!");

        this.transform.parent = null;
        this.transform.position = new Vector2(transform.position.x, transform.position.y);
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

        // If the hits that have been done with the weapon reach the limit
        if (hitsDone == hitsToBreak)
        {
            broken = true;
        }
        return broken;
    }

    protected void BreakItem()
    {
        Debug.Log("Weapon was destroyed!");
        // Fully destroy this object
        Destroy(gameObject);
    }

    // Used to detect which object will pick up the weapon, sets the hand to the hand of that GameObject
    protected void OnTriggerEnter2D(Collider2D entity)
    {
        if(entity.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hand = entity.transform.Find("handPlayer").gameObject;
        }
    }

    // Once the object exits, the hand is set to null
    protected void OnTriggerExit2D(Collider2D entity)
    {
        hand = null;
    }
}
