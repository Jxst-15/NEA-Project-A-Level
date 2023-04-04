public class HealthBarManager : BarManager
{
    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxHealth;
        barSlider.value = PlayerData.instance.currentHealth;
    }
}
