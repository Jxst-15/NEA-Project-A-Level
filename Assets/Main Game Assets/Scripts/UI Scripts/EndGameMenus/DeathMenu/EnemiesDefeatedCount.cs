public class EnemiesDefeatedCount: UITextController
{
    protected override void UpdateText()
    {
        text.text = PlayerData.instance.enemiesDefeated.ToString();
    }
}
