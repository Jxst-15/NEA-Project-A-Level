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
    private List<Collider2D> interact = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        weaponHolding = gameObject.GetComponentInParent<PlayerCombat>();
    }

    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.N) && interact.Count != 0)
        {
            IInteractable toInteract = interact[0].gameObject.GetComponent<IInteractable>();
            //Prioritises the first item in the list to interact with
            switch (interact[0].gameObject.tag)
            {
                case "Weapons":
                    // Checks if player already holding weapon
                    if (weaponHolding.weaponHeld != true)
                    {
                        weaponHolding.weaponHeld = true;

                        // Sets the weapon GameObject variable in PlayerCombat to the GameObject that the collider is attached to
                        weaponHolding.weapon = interact[0].gameObject;
                    }
                    else
                    {
                        Debug.Log("Already holding weapon");
                    }
                    break;
            }
            // Calls the interact method for the first item in the list
            toInteract.Interact();
        }
    }

    void OnTriggerEnter2D(Collider2D toInteract)
    {
        if (!interact.Contains(toInteract))
        {
            // Save points take priority
            if (toInteract.gameObject.tag == "SavePoint")
            {
                interact.Add(toInteract);
            }
            // If the item is not already in the list, it's a weapon and the player is not holding anything currently
            else if (toInteract.gameObject.tag == "Weapons" && weaponHolding.weaponHeld == false)
            {
                // Add to list
                interact.Add(toInteract);
            }
        }
    }

    void OnTriggerExit2D(Collider2D toInteract)
    {
        if(interact.Contains(toInteract))
        {
            interact.Remove(toInteract);
        }
    }
}
