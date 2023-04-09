using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{
    #region Fields
    #region Object References
    private readonly System.Random rng = new System.Random();

    public GameObject[] weapons = new GameObject[2]; // An array containing prefabs of the available enemy weapons
    #endregion

    #region Gameobjects
    public GameObject hand;
    #endregion

    #region Script References
    private EnemyCombat enemyCombat;
    private EnemyWCHandler enemyWCHandler;
    #endregion
    #endregion

    #region Unity Methods
    private void Awake()
    {
        enemyCombat = GetComponent<EnemyCombat>();
        enemyWCHandler = GetComponent<EnemyWCHandler>();
    }

    private void OnEnable()
    {
        DecideIfHolding();
    }
    #endregion

    // Uses rng to determine whether or not the enemy will hold a weapon
    private void DecideIfHolding()
    {
        int rand = rng.Next(1, 11);

        if (rand == 10)
        {
            GameObject weapon = Instantiate(ChooseWeapon()); // Creates the object so it can be used by the enemy

            weapon.GetComponent<Weapon>().hand = hand;
            weapon.GetComponent<Weapon>().Interact();
            
            enemyWCHandler.weapon = weapon;
            enemyWCHandler.weaponHeld = true;
            enemyWCHandler.weaponScript = weapon.GetComponent<Weapon>();
        }
        else
        {
            Debug.Log("Will not hold weapon");
        }
    }

    private GameObject ChooseWeapon()
    {
        int rand = rng.Next(1, 7);
        int index = 0;
        
        if (1 <= rand && rand <= 5)
        {
            index = 0; // Pole
        }
        else if (rand == 6 || rand == 7)
        {
            index = 1; // Sword
        }

        GameObject weapon = weapons[index]; // Access the array for the weapon

        return weapon;
    }
}
