using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class EndGameMenu : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject menuScreen;
    #endregion

    #region Script References
    protected PauseMenu pausing;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pausing = GetComponent<PauseMenu>();

        menuScreen.SetActive(false);
    }

    // Update is called once per frame
    protected abstract void Update();
    #endregion

    public void RestartButton()
    {
        menuScreen.SetActive(false);
        pausing.RestartButton();
    }

    public void RestartFromSave()
    {
        menuScreen.SetActive(false);
        Debug.Log("Restarting from save");

        // Loads the saved data
        StartCoroutine(ConnectionHandler.instance.LoadGame(PlayerData.instance.username));

        // Restarts the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        menuScreen.SetActive(false);
        pausing.QuitButton();
    }
}
