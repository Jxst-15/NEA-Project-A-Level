using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    #region Fields
    #region GameObject References

    #endregion

    #region Variables
    
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    public void PlayButton()
    {
        SceneManager.LoadScene("Main Game");
    }

    // Opens the save menu where player can access their saves
    public void SavesButton()
    {

    }

    // Code for opening settings
    public void SettingsButton()
    {
        // Load settings scene
    }

    // Quits the game
    public void QuitButton()
    {

    }
}
