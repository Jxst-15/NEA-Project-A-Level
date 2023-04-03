using System.Linq;
using UnityEngine;

// This is an abstract class meaning that this cannot be instantiated therefore it only serves the purpose of 
// a way for the other weapons to inherit what methods this has in order to save copying code
public abstract class Weapon : MonoBehaviour, IInteractable
{
    // Enum for all the weapon types within the game
    public enum WeaponType
    {
        Throwable,
        Pole,
        BladedWeapon
    }

    #region Fields
    #region Script References
    public WeaponAttackBox weaponAttack;
    public WeaponAttackBox uniqueWeaponAttack;
    #endregion

    #region GameObjects
    public GameObject attackBox;
    public GameObject uniqueAttackBox;
    
    public GameObject hand;
    protected GameObject objectHolding;
    #endregion

    #region Scale
    protected float scaleX;
    #endregion

    #region Variables
    protected WeaponType weaponType;
    protected string weaponName;
    protected float weaponRange;

    protected float nextWAttackTime;
    protected const float wAttackRate = 1.5f;

    protected int weaponLDmg;
    protected int weaponHDmg;
   
    protected float nextWUAttackTime;
    protected const float wUAttackRate = 0.25f;

    // How many times to attack in order to destroy the weapon
    protected int hitsDone;
    protected int hitsToBreak;
    #endregion

    #region Getters and Setters
    public int uniqueDmg
    { get; set; }

    public int stamDecWLAtk
    { get; protected set; }
    public int stamDecWHAtk
    { get; protected set; }
    public int stamDecWUEAtk
    { get; protected set; }
    public string getName() { return weaponName; }
    public void setName(string name) {  weaponName = name; }
    public int getHitsDone() { return hitsDone; }
    #endregion

    private Collider2D col;
    #endregion

    #region Unity Methods
    protected virtual void Awake()
    {
        weaponAttack = attackBox.GetComponent<WeaponAttackBox>();
        uniqueWeaponAttack = uniqueAttackBox.GetComponent<WeaponAttackBox>();

        col = this.GetComponent<Collider2D>();

        // scaleX = this.transform.localScale.x;
    }

    // I have made these methods virtual so they can be overriden in the actual weapon scripts
    protected virtual void Start()
    {
        attackBox.SetActive(false);
        uniqueAttackBox.SetActive(false);
        SetVariables();
    }

    protected virtual void Update()
    {
        CheckIfBroken();
    }
    #endregion

    protected abstract void SetVariables();

    // Set the position of the weapon to where the hand GameObject is and set its parent to the parent that the hand is attached to
    public void Interact()
    {
        if (hand != null)
        {
            Debug.Log("Weapon was picked up!");

            objectHolding = hand.transform.parent.gameObject; 

            attackBox.SetActive(true);
            uniqueAttackBox.SetActive(true);
            
            this.transform.parent = hand.transform; 

            // Gets the scale of the gameobject after it has been made a child object
            Vector3 newScale = this.transform.localScale;
            
            // Moves the weapon to where the hand is
            this.transform.position = hand.transform.position;

            Flip(newScale);

            col.enabled = false;

        }
    }

    // Ensures that the gameobject is facing the right way
    protected void Flip(Vector3 newScale)
    {
        Vector3 parentScale = hand.transform.parent.transform.localScale;

        if ((parentScale.x > 0 && newScale.x < 0) || (parentScale.x < 0 && newScale.x < 0))
        {
            newScale.x *= -1;
        }
        
        this.transform.localScale = newScale;
    }

    protected void DetectActionBox(Collider2D entity)
    {
        if (entity.gameObject.name == "actionBox")
        {
            if (entity.gameObject.transform.parent.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                hand = entity.transform.parent.Find("handPlayer").gameObject;
            }
        }
    }

    // Code to drop weapon
    public void DropItem()
    {
        Debug.Log("Weapon was dropped!");

        objectHolding = null;

        attackBox.SetActive(false);
        uniqueAttackBox.SetActive(false);

        hand = this.transform.parent.gameObject;
        
        // Moves the weapon down to the floor (the y pos of the gameobject holding the weapon)
        this.transform.position = new Vector2(hand.transform.position.x, hand.transform.parent.transform.position.y);
        
        this.transform.parent = null;

        // Makes it so it is interactable again
        col.enabled = true;
    }

    public virtual bool Attack(bool light)
    {
        bool objHit = false;
        if (Time.time >= nextWAttackTime)
        {
            int dmgToDeal;
            foreach (Collider2D hittableObj in weaponAttack.GetObjectsHit().ToList()) // ERROR MissingReferenceException:
                                                                                      // The object of type 'BoxCollider2D' has been destroyed but you are still trying to access it.
                                                                                      // Your script should either check if it is null or you should not destroy the object.
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
                DealDamage(hittableObj, dmgToDeal, 1);
            }
            nextWAttackTime = Time.time + 1f / wAttackRate;
        }
        return objHit;
    }

    // Code for using the unique attack for this weapon
    public virtual bool UniqueAttack()
    {
        bool objHit = false;      
        if (Time.time >= nextWUAttackTime && uniqueWeaponAttack.GetObjectsHit().Count != 0)
        {
            foreach (Collider2D hittableObj in uniqueWeaponAttack.GetObjectsHit().ToList())
            {
                if (hittableObj != null)
                {
                    objHit = true;
                    Debug.Log("Unique weapon attack");
                    DealDamage(hittableObj, uniqueDmg, 0);
                }
            }
            UpdateHitsDone(5);
            nextWUAttackTime = Time.time + 1f / wUAttackRate;
            nextWAttackTime = nextWUAttackTime;
        }
        return objHit;
    }

    protected void DealDamage(Collider2D toDmg, int dmg, int toUpdateHits)
    {
        toDmg.GetComponent<IDamageable>().TakeDamage(dmg, true);
        UpdateHitsDone(toUpdateHits);
    }

    public void UpdateHitsDone(int toUpdateHits)
    {
        hitsDone += toUpdateHits;
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

        objectHolding.GetComponent<IWeaponHandler>().SetWeaponToNull();
        objectHolding.GetComponent<IWeaponHandler>().SetWeaponHeld(false);
  
        // Fully destroy this object
        Destroy(gameObject);
    }

    #region OnTrigger Methods
    // Used to detect which object will pick up the weapon, sets the hand to the hand of that GameObject
    protected void OnTriggerEnter2D(Collider2D entity)
    {
        DetectActionBox(entity);
    }

    protected void OnTriggerStay2D(Collider2D entity)
    {
        DetectActionBox(entity);
    }

    // Once the object exits, the hand is set to null
    protected void OnTriggerExit2D(Collider2D entity)
    {
        hand = null;
    }
    #endregion
}
