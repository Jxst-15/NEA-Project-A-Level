using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    #region Fields
    #region Gameobject References
    public GameObject saveMenuObject;
    public GameObject healthBar;
    public GameObject player;
    #endregion

    #region Script References
    public SavePointHandler sp;
    private SaveHandler saveHandler;
    private PauseMenu pausing;
    private HealthBarManager healthBarManager;

    private PlayerPoints playerPoints;
    private PlayerStats playerStats;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        saveMenuObject.SetActive(false);
        pausing = GetComponent<PauseMenu>();
        saveHandler = GetComponent<SaveHandler>();
        healthBarManager = healthBar.GetComponent<HealthBarManager>();
        
        player = GameObject.FindWithTag("Player");
        playerPoints = player.GetComponent<PlayerPoints>();
        playerStats = player.GetComponent<PlayerStats>();
    }
    #endregion

    public void EnterSaveMenu()
    {
        saveMenuObject.SetActive(true);
        pausing.PauseGame();
    }

    // Will let player save the game
    public void SaveButton()
    {
        saveHandler.SaveGame(player.transform.position.x, player.transform.position.y, playerStats.currentHealth, playerStats.currentStamina, PlayerPoints.points);
    }

    // Will heal the player if player has sufficient points
    public void HealButton()
    {
        int cost = 5;
        if (PlayerPoints.points >= cost)
        {
            int toHealBy = 100;
            playerStats.Heal(toHealBy);
            playerPoints.ChangePoints(cost, "dec");
            healthBarManager.SetBarVal(playerStats.currentHealth);
        }
        else
        {
            Debug.Log("Insufficient Points");
        }
    }

    // Player leaves the save menu
    public void LeaveButton()
    {
        saveMenuObject.SetActive(false);
        pausing.ResumeGame();
    }
}
