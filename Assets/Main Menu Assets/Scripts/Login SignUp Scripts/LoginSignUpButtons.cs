using UnityEngine;
using TMPro;

public class LoginSignUpButtons : MonoBehaviour
{
    #region Fields
    #region Input Fields
    public TMP_InputField userInput;
    public TMP_InputField passInput;
    #endregion

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
        if (ConnectionHandler.instance.loggedIn == false)
        {
            user = "";
            pass = "";
        }
        else
        {
            logSuGO.SetActive(false);
        }
    }

    public void LoginButton()
    {
        logSuGO.SetActive(false);
        logFields.SetActive(true);

        Debug.Log(logFields.transform.childCount);

        SetUserInputs(logFields, "Username Login", "Password Login");
    }

    public void SignUpButton()
    {
        logSuGO.SetActive(false);
        signFields.SetActive(true);

        Debug.Log(signFields.transform.childCount);

        SetUserInputs(signFields, "Username SignUp", "Password SignUp");
    }

    // Both read strings are assigned to the text inputs
    public void ReadStringInputUser(string s)
    {
        user = s;
    }

    public void ReadStringInputPass(string s)
    {
        pass = s;
    }

    // Pressing the login button, assigned to the UI button
    public void AccountLoginButton()
    {
        Debug.Log(user + " " + pass);
        if (user != "" && pass != "")
        {
            logFields.SetActive(false);

            Debug.Log("Login");
            StartCoroutine(ConnectionHandler.instance.AttemptLogin(user, pass)); // Starts a coroutine for login
            ResetUserInputs();
        }
        else
        {
            // Shows error message to the user
            ConnectionHandler.instance.EmptyFieldError();
            logFields.SetActive(false);
        }
    }

    // Pressing the sign up button, assigned to the UI button
    public void AccountSignUpButton()
    {
        if (user != "" && pass != "")
        {
            signFields.SetActive(false);
            StartCoroutine(ConnectionHandler.instance.AttemptSignUp(user, pass));
            ResetUserInputs();
        }
        else
        {
            ConnectionHandler.instance.EmptyFieldError();
            signFields.SetActive(false);
        }
    }

    // Pressing the back button in the UI to visit a previous menu
    public void BackButton()
    {
        errorScreen.SetActive(false);
        ConnectionHandler.instance.error = false;
        logFields.SetActive(false);
        signFields.SetActive(false);

        ResetUserInputs();

        logSuGO.SetActive(true);
    }

    public void LogOutButton()
    {
        ConnectionHandler.instance.LogOut();
        logSuGO.SetActive(true);
    }

    // Gets the user input fields and sets them to their variables
    private void SetUserInputs(GameObject fields, string userField, string passField)
    {
        userInput = fields.transform.Find(userField).GetComponent<TMP_InputField>();
        passInput = fields.transform.Find(passField).GetComponent<TMP_InputField>();
        Debug.Log("Inputs Set");
    }

    // Makes it so the user input fields are empty 
    private void ResetUserInputs()
    {
        if (userInput != null && passInput != null)
        {
            userInput.text = "";
            passInput.text = "";

            userInput = null;
            passInput = null;

            user = "";
            pass = "";
            Debug.Log("Inputs reset");
        }
    }

    public void DeleteUser()
    {
        // Only works if the user is logged in
        if (ConnectionHandler.instance.loggedIn == true)
        {
            logSuGO.SetActive(true);
            StartCoroutine(ConnectionHandler.instance.DeleteUser(PlayerData.instance.username));
        }
        else
        {
            Debug.Log("Error, not logged in");
        }
    }
}
