using System.Collections;
using System.Collections.Generic;
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
        pauseMenuObject.SetActive(false);  
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
                    break;
                case false:
                    ResumeGame();
                    break;
            }
        }
    }
    #endregion

    public void PauseGame()
    {
        isPaused = true;
        gameUI.SetActive(false);
        backgroundObject.SetActive(true);
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        gameUI.SetActive(true);
        backgroundObject.SetActive(false);
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SettingsButton()
    {
        // Code for opening settings
        // Load settings scene
    }

    public void RestartButton()
    {
        // Code for restarting game
        // TEMPORARY
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        // Code for quitting game to menu
        // Load menu scene
    }
}
