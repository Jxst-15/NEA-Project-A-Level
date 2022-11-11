using UnityEngine;

public class PlayerStyleSwitch : MonoBehaviour
{
    #region Script References
    public PlayerStats stats;
    public PlayerCombat combat;
    #endregion

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        combat = GetComponent<PlayerCombat>();
    }

    public void SwitchStyle()
    {
        // Slows down game time by half
        Time.timeScale = 0.5f;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            combat.fightStyle = "Iron Fist";
            stats.lDmg = 50;
            stats.lDmg = 75;

            combat.attackRate = 2;

            combat.stamDecLAttack = 15;
            combat.stamDecHAttack = 20;
            combat.stamDecThrow = 35;
            combat.healthDecBlock = 4;
            Debug.Log("Current Style Is: " + combat.fightStyle);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            combat.fightStyle = "Boulder Style";
            stats.lDmg = 70;
            stats.hDmg = 95;

            combat.attackRate = 1f;

            combat.stamDecLAttack = 25;
            combat.stamDecHAttack = 30;
            combat.stamDecThrow = 45;
            combat.healthDecBlock = 2;
            Debug.Log("Current Style Is: " + combat.fightStyle);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            combat.fightStyle = "Grass Style";
            stats.lDmg = 30;
            stats.hDmg = 55;

            combat.attackRate = 3f;

            combat.stamDecLAttack = 5;
            combat.stamDecHAttack = 10;
            combat.stamDecThrow = 25;
            combat.healthDecBlock = 6;
            Debug.Log("Current Style Is: " + combat.fightStyle);
        }
    }
}
