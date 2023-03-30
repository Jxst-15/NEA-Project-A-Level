public class HighestComboCount : ComboController
{
    protected override void UpdateText()
    {
        text.text = playerCombat.comboMeter.highestCombo.ToString();
    }
}
