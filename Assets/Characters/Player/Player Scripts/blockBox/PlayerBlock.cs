using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private PlayerCombat combatScript;
    private int stamDecBlock;
    private int healthDecBlock;

    // Start is called before the first frame update
    private void Start()
    {
        combatScript = GetComponentInParent<PlayerCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        // Negate all damage taken by enemy attacks
        // For each attack that hits player in this state
        // Reduce stamina by stamDecBlock
        // Reduce health by healthDecBlock
    }
}
