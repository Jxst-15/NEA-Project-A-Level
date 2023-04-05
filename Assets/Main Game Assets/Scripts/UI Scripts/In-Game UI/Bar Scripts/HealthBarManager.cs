public class HealthBarManager : BarManager
{
    #region Unity Methods
    protected override void Update()
    {
        SetBarVal(playerStats.currentHealth);
    }
    #endregion

    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxHealth;
        barSlider.value = PlayerData.instance.currentHealth;
    }
}
