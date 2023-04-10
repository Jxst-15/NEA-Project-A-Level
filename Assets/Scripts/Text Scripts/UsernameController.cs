public class UsernameController : UITextController
{
    protected override void UpdateText()
    {
        text.text = PlayerData.instance.username;
    }
}
