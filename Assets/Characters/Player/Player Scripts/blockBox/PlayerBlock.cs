using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private PlayerCombat combatScript;

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
}
