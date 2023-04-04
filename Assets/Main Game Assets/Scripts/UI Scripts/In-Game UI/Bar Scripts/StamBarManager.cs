public class StamBarManager : BarManager
{
    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxStamina;
        barSlider.value = PlayerData.instance.currentStamina;
    }
}
