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
    public GameObject dropPoint;
    #endregion

    #region Variables
    protected WeaponType weaponType;
    protected string weaponName;
    protected float weaponRange;

    protected float nextWAttackTime;
    protected const float wAttackRate = 2f;

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
        this.transform.parent = hand.transform;
        if (hand.transform.parent.localScale.x < 0)
        {
            this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
        }
        this.transform.position = new Vector2(hand.transform.position.x, hand.transform.position.y);
    }

    public void DropItem()
    {
        // Code to drop weapon
        Debug.Log("Weapon was dropped!");

        this.transform.parent = null;
        this.transform.position = new Vector2(dropPoint.transform.position.x, dropPoint.transform.position.y);
        this.hand = null;
        this.dropPoint = null;
    }

    public virtual void Attack()
    {
        if (Time.time >= nextWAttackTime)
        {
            Debug.Log("Attacked with weapon!");
            UpdateHitsDone();

            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
    }

    public abstract void UniqueAttack();

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
        if (entity.gameObject.name == "actionBox")
        {
            if (entity.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hand = entity.transform.parent.Find("handPlayer").gameObject;
                dropPoint = entity.transform.parent.Find("actionBox").gameObject;
            }
        }
    }

    // Once the object exits, the hand is set to null
    protected void OnTriggerExit2D(Collider2D entity)
    {
        hand = null;
        dropPoint = null;
    }
}
