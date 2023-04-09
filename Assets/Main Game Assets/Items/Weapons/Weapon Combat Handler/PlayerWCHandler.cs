using System.Linq;
using UnityEngine;

public class PlayerWCHandler : WeaponCombatHandler
{
    #region Fields
    public PlayerStats playerStats;
    public PlayerCombat playerCombat;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerCombat = GetComponent<PlayerCombat>();
    }
    #endregion
    
    public override void WeaponAttackLogic()
    {
        if (Input.GetButtonDown("lAttack") || Input.GetButtonDown("hAttack") || Input.GetKeyDown(KeyCode.L))
        {
            playerCombat.attacking = true;
            playerCombat.parryable = true;

            int toDecBy = 0;
            bool unique = false;
            bool lightAtk = false;

            if (Input.GetButtonDown("lAttack"))
            {
                lightAtk = true;
                toDecBy = weaponScript.stamDecWLAtk;
            }
            else if (Input.GetButtonDown("hAttack"))
            {
                lightAtk = false;
                toDecBy = weaponScript.stamDecWHAtk;
            }
            else if (Input.GetKeyDown(KeyCode.L)) // If the button is the unique attack button
            {
                unique = true;
                toDecBy = weaponScript.stamDecWUEAtk;
            }

            WeaponAttack(lightAtk, unique, toDecBy);
           
            playerCombat.attacking = false;
            playerCombat.parryable = false;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            DropWeapon();
        }
    }

    protected override void UseNormalWeaponAtk(int toDecBy)
    {
        // Get the number of objects hit by attack and increase comboCount by amount
        playerCombat.comboCount += weaponScript.weaponAttack.GetObjectsHit().ToList().Count;
        playerCombat.comboMeter.ComboStart(playerCombat.comboCount);
        
        playerStats.AffectCurrentStamima(toDecBy, "dec");
    }

    protected override void UseUniqueWeaponAtk(int toDecBy)
    {
        // Get the number of objects hit by attack and increase comboCount by amount
        playerCombat.comboCount += weaponScript.uniqueWeaponAttack.GetObjectsHit().ToList().Count;
        playerCombat.comboMeter.ComboStart(playerCombat.comboCount);
        
        playerStats.AffectCurrentStamima(toDecBy, "dec");
    }

    private void DropWeapon()
    {
        if (weaponHeld == true)
        {
            weaponScript.DropItem();
            weapon = null;
            weaponHeld = false;
            weaponScript = null;

            // Player is now able to switch fighting styles again
            playerCombat.canSwitch = true;
        }
    }

}
