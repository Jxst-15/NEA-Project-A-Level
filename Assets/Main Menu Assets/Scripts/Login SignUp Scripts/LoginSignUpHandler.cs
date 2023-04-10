using UnityEngine;

public class LoginSignUpHandler : MonoBehaviour
{
    #region Fields
    #region Script References
    public LoginSignUpButtons buttons;
    #endregion
    #endregion

    void Awake()
    {
        buttons = GetComponent<LoginSignUpButtons>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ConnectionHandler.instance.error == false)
        {
            if (ConnectionHandler.instance.loggedIn == true)
            {
                // Show login screen
                buttons.loggedIn.SetActive(true);
            }
            else
            {
                buttons.loggedIn.SetActive(false);
            }
        }
        else
        {
            // Show the error screen
            buttons.errorScreen.SetActive(true);
        }
    }
}
