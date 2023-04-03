using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Will handle the communcation between Unity and the php scripts provided by URL
public class ConnectionHandler : MonoBehaviour
{
    #region Fields
    #region Getters and Setters
    public string url
    { get; set; }
    public string dataString
    { get; private set; }
    public bool error
    { get; set; }
    public string errorMsg
    { get; private set; }
    public bool loggedIn
    { get; private set; }
    #endregion

    public WWWForm form;
    public static ConnectionHandler instance;

    #region URLs
    private const string loginURL = "http://localhost/compSciNeaDB/login.php";
    private const string signupURL = "http://localhost/compSciNeaDB/signup.php";
    private const string saveURL = "http://localhost/compSciNeaDB/saving.php";
    #endregion
    #endregion

    #region Unity Methods
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        dataString = "";
        errorMsg = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    public void OpenConn(string url, WWWForm form)
    {
        if (url != null)
        {
            StartCoroutine(GetRequest(url, form));
        }
        else
        {
            Debug.Log("Error: URL string is empty");
        }
    }

    public void EmptyFieldError()
    {
        error = true;
        errorMsg = "Error: username or password field was empty";
    }

    private IEnumerator GetRequest(string url, WWWForm form)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    errorMsg = "Error: Connection Failed";
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    errorMsg = "Error: " + webRequest.error;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    errorMsg = "Error: HTTP Error: " + webRequest.error;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    dataString = webRequest.downloadHandler.text;
                    break;
            }
        }
    }

    public IEnumerator AttemptLogin(string u, string p)
    {
        form = new WWWForm();

        form.AddField("username", u);
        form.AddField("password", p);

        yield return GetRequest(loginURL, form);
      
        string[] data = dataString.Split("*");

        int success = Convert.ToInt32(data[0]);
        if (success == 1)
        {
            loggedIn = true;

            PlayerData.instance.username = u;
            // PlayerData.instance.points = Convert.ToInt32(data[1]);
        }
        else
        {
            error = true;
            errorMsg = "Error: username or password entered was not correct";
        }

        dataString = "";
    }

    public IEnumerator AttemptSignUp(string u, string p)
    {
        form = new WWWForm();

        form.AddField("username", u);
        form.AddField("password", p);

        yield return GetRequest(signupURL, form);

        if (dataString == "1")
        {
            loggedIn = true;
            PlayerData.instance.username = u;
        }
        else
        {
            error = true;
            errorMsg = "Error: The user entered already exists";
        }
        dataString = "";
    }

    public void LogOut()
    {
        loggedIn = false;
    }
    public IEnumerator SaveGame(string saveName, int health, int stamina, float posX, float posY, int points)
    {
        Debug.Log("Running SaveGame");
        form = new WWWForm();

        form.AddField("username", PlayerData.instance.username);
        form.AddField("points", points);
        form.AddField("currentHealth", health);
        form.AddField("currentStamina", stamina);
        form.AddField("posX", posX.ToString());
        form.AddField("posY", posY.ToString());

        yield return GetRequest(saveURL, form);

        Debug.Log("\n" + dataString);

        string[] data = dataString.Split("*");

        int success = Convert.ToInt32(data[0]); // Inout string was not in correct format
        if (success == 1)
        {
            Debug.Log("Save was a success!");
        }
        else
        {
            Debug.Log("Error");
        }
        Debug.Log("\n" + dataString);
        dataString = "";
    }
}
