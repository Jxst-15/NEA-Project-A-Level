public class FinishMenu : EndGameMenu
{
    #region Fields
    #region Getters and Setters
    public bool finishedGame
    { get; set; }
    #endregion
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        finishedGame = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (finishedGame == true)
        {
            pausing.PauseGame();
            menuScreen.SetActive(true);
        }
    }
    #endregion
}
