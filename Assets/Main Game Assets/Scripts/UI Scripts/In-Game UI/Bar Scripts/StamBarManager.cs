public class StamBarManager : BarManager
{
    #region Unity Methods
    protected override void Update()
    {
        SetBarVal(playerStats.currentStamina);
    }
    #endregion

    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxStamina;
        barSlider.value = PlayerData.instance.currentStamina;
    }
}
