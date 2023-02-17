using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // For the weaponHeld variable
    #region Script References
    public PlayerCombat weaponHolding;
    #endregion

    // A list containing all items that are within the actionBox collider
    private List<Collider2D> items = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        weaponHolding = gameObject.GetComponentInParent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            IInteractable toInteract = items[0].gameObject.GetComponent<IInteractable>();
            switch (items[0].gameObject.tag)
            {
                case "Weapons":
                    // Checks if player already holding weapon
                    if (weaponHolding.weaponHeld != true)
                    {
                        //Uses the first item in the list to pick up weapon
                        // Needs to check if the weapon is in range, that's where the null reference exception is from future me :)
                        toInteract.Interact();
                        weaponHolding.weaponHeld = true;
                        
                        // Sets the weapon GameObject variable in PlayerCombat to the GameObject that the collider is attached to
                        weaponHolding.weapon = items[0].gameObject;
                    }
                    else
                    {
                        Debug.Log("Already holding weapon");
                    }
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D toInteract)
    {
        // If the item is not already in the list, it's a weapon and the player is not holding anything currently
        if (!items.Contains(toInteract) && toInteract.gameObject.tag == "Weapons" && weaponHolding.weaponHeld == false)
        {
            // Add to list
            items.Add(toInteract);
        }
    }

    void OnTriggerExit2D(Collider2D toInteract)
    {
        if(items.Contains(toInteract))
        {
            // Debug.Log("Weapon removed from list");
            items.Remove(toInteract);
        }
    }
}
