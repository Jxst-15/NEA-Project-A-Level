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

    #region Variables
    public string user
    { get; set; }
    private string pass;
    #endregion

    #endregion

    private void Awake()
    {
        user = LoginSignUpHandler.defVal;
        pass = "";

        if (ConnectionHandler.instance.loggedIn == true)
        {
            logSuGO.SetActive(false);
        }
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
        Debug.Log(user + " " + pass);
        if (user != "" && pass != "")
        {
            logFields.SetActive(false);

            Debug.Log("Login");
            StartCoroutine(ConnectionHandler.instance.AttemptLogin(user, pass));
        }
        else
        {
            ConnectionHandler.instance.EmptyFieldError();
            logFields.SetActive(false);
        }
    }

    public void AccountSignUpButton()
    {
        if (user != "" && pass != "")
        {
            signFields.SetActive(false);

            StartCoroutine(ConnectionHandler.instance.AttemptSignUp(user, pass));
        }
        else
        {
            ConnectionHandler.instance.EmptyFieldError();
            signFields.SetActive(false);
        }
    }

    public void BackButton()
    {
        errorScreen.SetActive(false);
        ConnectionHandler.instance.error = false;
        logFields.SetActive(false);
        signFields.SetActive(false);

        logSuGO.SetActive(true);
    }

    public void LogOuButtont()
    {
        user = "";
        ConnectionHandler.instance.LogOut();
        logSuGO.SetActive(true);
    }
}
