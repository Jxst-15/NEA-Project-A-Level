public class HealthBarManager : BarManager
{
    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxHealth;
        barSlider.value = playerStats.currentHealth;
    }
}
