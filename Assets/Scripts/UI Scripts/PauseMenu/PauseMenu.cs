using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    #region GameObject References
    public GameObject backgroundObject;
    public GameObject pauseMenuObject;
    #endregion

    #region Variables
    [SerializeField] public static bool isPaused;
    #endregion

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

    public void PauseGame()
    {
        isPaused = true;
        backgroundObject.SetActive(true);
        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        backgroundObject.SetActive(false);
        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SettingsButton()
    {
        // Code for opening settings
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
    }
}
