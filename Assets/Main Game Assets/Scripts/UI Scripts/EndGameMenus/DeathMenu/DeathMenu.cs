using UnityEngine;

public class DeathMenu : EndGameMenu
{
    #region Fields
    #region Script References
    private PlayerStats playerStats;
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (playerStats.dead == true)
        {
            pausing.PauseGame();
            menuScreen.SetActive(true);
        }
    }
    #endregion
}
