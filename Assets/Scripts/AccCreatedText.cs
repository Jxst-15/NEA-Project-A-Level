public class AccCreatedText : UITextController
{
    protected override void UpdateText()
    {
        text.text = "Account Created: " + PlayerData.instance.accCreated;
    }
}
