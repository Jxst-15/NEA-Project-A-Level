public class HealthBarManager : BarManager
{
    private void Update()
    {
        SetBarVal(playerStats.currentHealth);
    }

    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxHealth;
        barSlider.value = PlayerData.instance.currentHealth;
    }
}
