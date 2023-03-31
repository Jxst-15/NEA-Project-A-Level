public class EnemiesDefeatedCount: ComboController
{
    protected override void UpdateText()
    {
        text.text = PlayerData.instance.enemiesDefeated.ToString();
    }
}
