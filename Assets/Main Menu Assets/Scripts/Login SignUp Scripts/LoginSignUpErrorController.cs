public class LoginSignUpErrorController : UITextController
{
    protected override void UpdateText()
    {
        text.text = "Error: " + ConnectionHandler.instance.errorMsg;
    }
}
