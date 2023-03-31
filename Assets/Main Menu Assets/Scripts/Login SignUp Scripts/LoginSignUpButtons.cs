using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoginSignUpButtons : MonoBehaviour
{
    #region Fields
    #region Gameobjects
    public GameObject logSuGO;
    public GameObject errorScreen;

    public GameObject logFields;
    public GameObject signFields;

    public GameObject loggedIn;
    #endregion

    #region Script References
    private LoginSignUpHandler handler;
    #endregion

    #region Variables
    private string user;
    private string pass;
    #endregion

    #endregion

    private void Awake()
    {
        handler = GetComponent<LoginSignUpHandler>();
    }

    public void LoginButton()
    {
        logSuGO.SetActive(false);
        logFields.SetActive(true);
    }

    public void SignUpButton()
    {
        logSuGO.SetActive(false);
        signFields.SetActive(true);
    }

    public void ReadStringInputUser(string s)
    {
        user = s;
    }

    public void ReadStringInputPass(string s)
    {
        pass = s;
    }

    public void AccountLoginButton()
    {
        logFields.SetActive(false);

        handler.Login(user, pass);
    }

    public void AccountSignInButton()
    {
        handler.SignIn(user, pass);
    }

    public void BackButton()
    {
        errorScreen.SetActive(false);
        logFields.SetActive(false);
        signFields.SetActive(false);

        logSuGO.SetActive(true);
    }
}
