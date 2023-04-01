using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    #region Fields
    #region GameObject References
    public GameObject needLogin;
    #endregion

    #region Variables
    
    #endregion
    #endregion

    #region Unity Methods
    // Update is called once per frame
    void Update()
    {
        if (ConnectionHandler.instance.loggedIn == true)
        {
            needLogin.SetActive(false);
        }
    }
    #endregion

    public void PlayButton()
    {
        if (ConnectionHandler.instance.loggedIn == false)
        {
            needLogin.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Main Game");
        }
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
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
