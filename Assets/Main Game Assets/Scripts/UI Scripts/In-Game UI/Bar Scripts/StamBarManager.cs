public class StamBarManager : BarManager
{
    private void Update()
    {
        SetBarVal(playerStats.currentStamina);
    }

    protected override void SetValues()
    {
        barSlider.maxValue = playerStats.maxStamina;
        barSlider.value = PlayerData.instance.currentStamina;
    }
}
