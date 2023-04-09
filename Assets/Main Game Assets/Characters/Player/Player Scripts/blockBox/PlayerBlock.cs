using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP
// Want to fix block implementation/ improve it
public class PlayerBlock : MonoBehaviour
{
    #region Fields
    private List<Collider2D> toBlock = new List<Collider2D>();

    #region Getters and Setters
    public int stamDecBlock
    { get; set; }
    public int healthDecBlock
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    private void Start()
    {
        stamDecBlock = 5;
        healthDecBlock = 2;
    }

    private void Update()
    {
        // If the player is not blocking, H key has been released
        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("No Longer Blocking");
            foreach(Collider2D c in toBlock)
            {
                // For each enemy in the toBlock collider, set gettingBlocked to false so they can attack normally
                if (c != null)
                {
                    c.GetComponent<EnemyCombat>().gettingBlocked = false;
                }
            }
        }
    }
    #endregion

    public void Block()
    {
        foreach(Collider2D c in toBlock)
        {
            if (c != null)
            {
                c.GetComponent<EnemyCombat>().gettingBlocked = true;
            }
        }
    }

    #region OnTrigger Methods
    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (!toBlock.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy") && enemy.gameObject.name != "groundBox")
        {
            // Adds enemy to list so they can be blocked if player chooses
            toBlock.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D enemy)
    {
        if (toBlock.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy.GetComponent<EnemyCombat>().gettingBlocked = false;
            toBlock.Remove(enemy);
        }
    }
    #endregion
}
