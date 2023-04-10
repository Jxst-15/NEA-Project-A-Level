public class LoginSignUpErrorController : UITextController
{
    protected override void UpdateText()
    {
        text.text = ConnectionHandler.instance.errorMsg;
    }
}
