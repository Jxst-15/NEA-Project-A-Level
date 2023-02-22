using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WIP
// Want to fix block implementation/ improve it
public class PlayerBlock : MonoBehaviour
{
    #region Script References
    private PlayerCombat combatScript;

    private EnemyCombat blockEnemy;
    #endregion

    private List<Collider2D> toBlock = new List<Collider2D>();

    #region Getters and Setters
    public int stamDecBlock
    { get; set; }
    public int healthDecBlock
    { get; set; }
    #endregion

    private void Awake()
    {
        combatScript = GetComponentInParent<PlayerCombat>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        stamDecBlock = 5;
        healthDecBlock = 2;
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.H))
        {
            foreach(Collider2D c in toBlock)
            {
                c.GetComponent<EnemyCombat>().gettingBlocked = false;
            }
        }
    }

    public void Block()
    {
        foreach(Collider2D c in toBlock)
        {
            c.GetComponent<EnemyCombat>().gettingBlocked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (!toBlock.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy") && enemy.gameObject.name != "groundBox")
        {
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
}
