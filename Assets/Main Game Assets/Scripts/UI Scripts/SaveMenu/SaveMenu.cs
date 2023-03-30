using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    #region Fields
    #region Gameobject References
    public GameObject saveMenuObject;
    #endregion

    #region Script References
    public SavePointHandler sp;
    private SaveHandler saveHandler;
    private PauseMenu pausing;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        saveMenuObject.SetActive(false);
        pausing = GetComponent<PauseMenu>();
        saveHandler = GetComponent<SaveHandler>();
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
        saveHandler.SaveGame();
    }

    // Will heal the player if player has sufficient points
    public void HealButton()
    {

    }

    // Player leaves the save menu
    public void LeaveButton()
    {
        saveMenuObject.SetActive(false);
        pausing.ResumeGame();
    }
}
