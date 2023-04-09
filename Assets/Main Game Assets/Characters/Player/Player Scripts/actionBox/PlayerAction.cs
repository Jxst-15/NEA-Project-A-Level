using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    #region Fields
    // For the weaponHeld variable
    #region Script References
    public PlayerCombat playerCombat;
    public PlayerWCHandler playerWCHandler;
    #endregion

    #region Getters and Setters
    public bool canInteract
    { get; set; }
    #endregion

    // A list containing all items that are within the actionBox collider
    private List<Collider2D> interact = new List<Collider2D>();
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        playerCombat = gameObject.GetComponentInParent<PlayerCombat>();
        playerWCHandler = gameObject.GetComponentInParent<PlayerWCHandler>();
        canInteract = true;
    }
    #endregion

    // For interacting with objects in the game 
    public void Action()
    {
        if (Input.GetKeyDown(KeyCode.N) && interact.Count != 0 && canInteract == true)
        {
            IInteractable toInteract = interact[0].gameObject.GetComponent<IInteractable>();
            //Prioritises the first item in the list to interact with
            switch (interact[0].gameObject.tag)
            {
                case "Weapons":
                    WeaponInteract();
                    break;
            }
            // Ensures that player cannot pick up all items that are in the interact list
            interact.Clear();
            
            // Calls the interact method for the first item in the list
            toInteract.Interact(); 
        }
    }

    private void WeaponInteract()
    {
        // Checks if player already holding weapon
        if (playerWCHandler.weaponHeld == false)
        {
            // Checks if player is holding a weapon, if yes then set weaponHeld to true
            playerWCHandler.weaponHeld = true;

            playerWCHandler.weaponScript = interact[0].gameObject.GetComponent<Weapon>();

            // Sets the weapon GameObject variable in playerWCHandler to the GameObject that the collider is attached to
            playerWCHandler.weapon = interact[0].gameObject;
        }
        else
        {
            Debug.Log("Already holding weapon");
        }
    }

    #region OnTrigger Methods
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
            else if (toInteract.gameObject.layer == LayerMask.NameToLayer("Weapons") && playerWCHandler.weaponHeld == false)
            {
                // Add to list
                interact.Add(toInteract);
            }
        }
    }

    void OnTriggerExit2D(Collider2D toInteract)
    {
        if (interact.Contains(toInteract))
        {
            interact.Remove(toInteract);
        }
    }
    #endregion
}
