using UnityEngine;

public class LoginSignUpErrorController : UITextController
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateText()
    {
        text.text = ConnectionHandler.instance.errorMsg;
    }
}
