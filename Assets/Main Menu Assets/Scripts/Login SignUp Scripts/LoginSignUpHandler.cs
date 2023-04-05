using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginSignUpHandler : MonoBehaviour
{
    #region Fields
    #region Script References
    public LoginSignUpButtons buttons;
    private ConnectionHandler connHandler;
    #endregion

    #region Getters and Setters
    #endregion

    public const string defVal = "None";
    #endregion

    void Awake()
    {
        connHandler = GameObject.Find("DataHandler").GetComponent<ConnectionHandler>();
        buttons = GetComponent<LoginSignUpButtons>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ConnectionHandler.instance.error == false)
        {
            if (ConnectionHandler.instance.loggedIn == true)
            {
                buttons.loggedIn.SetActive(true);
            }
            else
            {
                buttons.loggedIn.SetActive(false);
            }
        }
        else
        {
            buttons.errorScreen.SetActive(true);
        }
    }
}
