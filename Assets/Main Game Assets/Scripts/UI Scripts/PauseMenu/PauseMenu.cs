using UnityEngine;
using UnityEngine.SceneManagement;

// WIP
public class PauseMenu : MonoBehaviour
{
    #region Fields
    #region GameObject References
    public GameObject gameUI;
    public GameObject backgroundObject;
    public GameObject pauseMenuObject;
    #endregion

    #region Variables
    [SerializeField] public static bool isPaused;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        backgroundObject.SetActive(false);
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            switch (isPaused)
            {
                case true:
                    PauseGame();
                    pauseMenuObject.SetActive(true);
                    break;
                case false:
                    ResumeGame();
                    break;
            }
        }
    }
    #endregion

    // Pauses the game
    public void PauseGame()
    {
        isPaused = true;
        gameUI.SetActive(false);
        backgroundObject.SetActive(true);
        Time.timeScale = 0f;
    }

    // Resumes the game
    public void ResumeGame()
    {
        isPaused = false;
        gameUI.SetActive(true);
        backgroundObject.SetActive(false);
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
    }

    // Code for opening settings
    public void SettingsButton()
    {
        // Load settings scene
    }

    // Code for restarting game
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Code for quitting game to menu
    public void QuitButton()
    {
        // Load menu scene
        ResumeGame();
        SceneManager.LoadScene("Main Menu");
    }
}
