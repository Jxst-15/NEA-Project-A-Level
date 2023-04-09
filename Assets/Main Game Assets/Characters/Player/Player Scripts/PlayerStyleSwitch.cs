using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStyleSwitch : MonoBehaviour
{
    #region Fields
    #region Script References
    private PlayerStats stats;
    private PlayerCombat playerCombat;
    private PlayerWCHandler playerWCHandler;
    private PlayerBlock blockStats;
    #endregion

    public static string fightStyle;

    public GameObject StyleSwitcher;
    #endregion

    #region Unity Methods
    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        playerCombat = GetComponent<PlayerCombat>();
        playerWCHandler = GetComponent<PlayerWCHandler>();
        blockStats = GetComponentInChildren<PlayerBlock>();
        
        fightStyle = "Iron Fist"; // Default start style
    }

    void Update()
    {
        // If the player is holding a weapon then cannot switch
        switch (playerWCHandler.weaponHeld)
        {
            case true:
                playerCombat.canSwitch = false;
                break;
            case false:
                playerCombat.canSwitch = true;
                break;
        }
    }
    #endregion

    public void SwitchStyle()
    {
        // Slows down game time by half
        Time.timeScale = 0.5f;

        StyleSwitcher.SetActive(true);

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetStatValues("Iron Fist", 50, 75, 2f, 15, 20, 35, 4);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetStatValues("Boulder Style", 70, 95, 1f, 25, 30, 45, 2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetStatValues("Grass Style", 30, 55, 3f, 5, 10, 25, 6);
        }
    }

    // Sets the appropriate values
    private void SetStatValues(string style, int lDmg, int hDmg, float atkR, int sDLA, int sDHA, int sDT, int hDB)
    {
        fightStyle = style;
        stats.lDmg = lDmg;
        stats.hDmg = hDmg;

        playerCombat.attackRate = atkR;

        playerCombat.stamDecLAttack = sDLA;
        playerCombat.stamDecHAttack = sDHA;
        playerCombat.stamDecThrow = sDT;
        blockStats.healthDecBlock = hDB;
        Debug.Log("Current Style Is: " + fightStyle);
    }
}
