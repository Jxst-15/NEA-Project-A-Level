using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    #region Script References
    private PlayerCombat combatScript;
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
        stamDecBlock = 10;
        healthDecBlock = 4;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        Block();
    }

    private void Block()
    {
        foreach(Collider2D c in toBlock)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D enemy)
    {
        if (!toBlock.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            toBlock.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D enemy)
    {
        if (toBlock.Contains(enemy) && enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            toBlock.Remove(enemy);
        }
    }
}
