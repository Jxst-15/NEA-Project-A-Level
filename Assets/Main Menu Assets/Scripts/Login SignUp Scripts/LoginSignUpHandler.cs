using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoginSignUpHandler : MonoBehaviour
{
    #region Fields
    #region Script References
    public ConnectionHandler connHandler;
    #endregion

    #region Getters and Setters
    public string dataString
    { get; set; }
    #endregion

    public WWWForm form;

    private const string loginURL = "http://localhost/compSciNeaDB/login.php";
    #endregion

    void Awake()
    {
        connHandler = GameObject.Find("DataHandler").GetComponent<ConnectionHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login(string user, string pass)
    {
        form = new WWWForm();

        form.AddField("username", user);
        form.AddField("password", pass);

        connHandler.OpenConn(loginURL, form);
        //Debug.Log(connHandler.dataString);

        //if (connHandler.dataString != "")
        //{
        //    string[] data = connHandler.dataString.Split('*');
        //    connHandler.ResetDataString();
        //    Debug.Log(data[0]);
        //}
    }

    public string GetURL()
    {
        return loginURL;
    }

    //public IEnumerator Login(string user, string pass)
    //{
    //    bool success = false;
    //    form = new WWWForm();

    //    form.AddField("username", user);
    //    form.AddField("password", pass);

    //    connHandler.OpenConn(loginURL, form);

    //    if (connHandler.dataString != "")
    //    {
    //        string[] data = connHandler.dataString.Split('*');
    //        connHandler.ResetDataString();
    //        Debug.Log(data[0]);
    //        success = true;
    //    }

    //    yield return success;
    //}

    public void SignIn(string user, string pass)
    {

    }
}
