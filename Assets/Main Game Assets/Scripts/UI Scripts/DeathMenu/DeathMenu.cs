using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject deathScreen;
    #endregion

    #region Script References
    private PlayerStats playerStats;
    private PauseMenu pausing;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        pausing = GetComponent<PauseMenu>();

        deathScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStats.dead == true)
        {
            pausing.PauseGame();
            deathScreen.SetActive(true);
        }
    }
    #endregion

    public void RestartButton()
    {
        deathScreen.SetActive(false);
        pausing.RestartButton();
    }

    public void RestartFromSave()
    {
        deathScreen.SetActive(false);
        Debug.Log("Restarting from save");
        
        StartCoroutine(ConnectionHandler.instance.LoadGame(PlayerData.instance.username));
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        deathScreen.SetActive(false);
        pausing.QuitButton();
    }
}
