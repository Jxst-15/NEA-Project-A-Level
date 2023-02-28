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

    #region Script References
    public WeaponAttackBox weaponAttack;
    #endregion

    #region GameObjects
    public GameObject attackBox;
    
    public GameObject hand;
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

    protected virtual void Awake()
    {
        weaponAttack = attackBox.GetComponent<WeaponAttackBox>();
    }

    // I have made these methods virtual so they can be overriden in the actual weapon scripts
    protected virtual void Start()
    {
        attackBox.SetActive(false);
        SetVariables();
    }

    protected virtual void Update()
    {
        CheckIfBroken();
    }

    protected abstract void SetVariables();

    // WIP
    // Set the position of the weapon to where the hand GameObject is and set its parent to the parent that the hand is attached to
    public void Interact()
    {
        if (hand != null)
        {
            Debug.Log("Weapon was picked up!");

            attackBox.SetActive(true);
            this.transform.parent = hand.transform;
            if (hand.transform.parent.localScale.x > 0 && this.transform.localScale.x < 0)
            {
                this.transform.localScale = new Vector2(this.transform.localScale.x, this.transform.localScale.y);
            }
            else if (hand.transform.parent.localScale.x < 0 && this.transform.localScale.x < 0)
            {
                this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
            }
            this.transform.position = new Vector2(hand.transform.position.x, hand.transform.position.y);
        }
    }

    // WIP
    public void DropItem(GameObject dropPoint)
    {
        // Code to drop weapon
        Debug.Log("Weapon was dropped!");

        attackBox.SetActive(false);
        this.transform.parent = null;
        this.transform.position = new Vector2(dropPoint.transform.position.x, dropPoint.transform.position.y);
        this.hand = null;
    }

    public virtual bool Attack(bool light)
    {
        bool objHit = false;
        if (Time.time >= nextWAttackTime)
        {
            int dmgToDeal;
            foreach (Collider2D hittableObj in weaponAttack.GetObjectsHit())
            {
                objHit = true;
                switch (light)
                {
                    case true:
                        // Do light damage
                        dmgToDeal = weaponLDmg;
                        Debug.Log("Light weapon attack");
                        break;
                    case false:
                        // Do heavy damage
                        dmgToDeal = weaponHDmg;
                        Debug.Log("Heavy weapon attack");
                        break;
                }
                DealDamage(hittableObj, dmgToDeal);
            }
            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
        return objHit;
    }

    public abstract void UniqueAttack();

    private void DealDamage(Collider2D toDmg, int dmg)
    {
        toDmg.GetComponent<IDamageable>().TakeDamage(dmg);
        UpdateHitsDone();
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
        attackBox.SetActive(false);
        // Fully destroy this object
        Destroy(gameObject);
    }

    #region OnTrigger Methods
    // Used to detect which object will pick up the weapon, sets the hand to the hand of that GameObject
    protected void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.gameObject.name == "actionBox")
        {
            if (entity.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hand = entity.transform.parent.Find("handPlayer").gameObject;
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D entity)
    {
        if (entity.gameObject.name == "actionBox")
        {
            if (entity.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hand = entity.transform.parent.Find("handPlayer").gameObject;
            }
        }
    }

    // Once the object exits, the hand is set to null
    protected void OnTriggerExit2D(Collider2D entity)
    {
        hand = null;
    }
    #endregion
}
