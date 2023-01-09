using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    // For the weaponHeld variable
    #region Script References
    public PlayerCombat heldStatus;
    #endregion

    // A list containing all items that are within the actionBox collider
    private List<Collider2D> items = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        heldStatus = gameObject.GetComponentInParent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D toInteract)
    {
        // If the item is not already in the list, it's a weapon and the player is not holding anything currently
        if (!items.Contains(toInteract) && toInteract.gameObject.tag == "Weapons" && heldStatus.weaponHeld == false)
        {
            // Add to list
            items.Add(toInteract);

            //Uses the first item in the list to pick up weapon
            // Needs to check if the weapon is in range, that's where the null reference exception is from future me :)
            items[0].gameObject.GetComponent<IInteractable>().Interact();
            heldStatus.weaponHeld = true;

            // Sets the weapon GameObject variable in PlayerCombat to the GameObject that the collider is attached to
            heldStatus.weapon = toInteract.gameObject;
        }
        else if (heldStatus.weaponHeld == true)
        {
            Debug.Log("Cannot pick up, already holding a weapon");
        }
    }

    void OnTriggerExit2D(Collider2D toInteract)
    {
        if(items.Contains(toInteract) && toInteract.gameObject.tag == "Weapons")
        {
            Debug.Log("Weapon removed from list");
            items.Remove(toInteract);
        }
    }
}
