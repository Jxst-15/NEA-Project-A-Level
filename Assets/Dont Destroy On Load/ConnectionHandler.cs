using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Will handle the communication between Unity and the php scripts provided by URL
public class ConnectionHandler : MonoBehaviour
{
    #region Fields
    #region Object References
    public WWWForm form;
    public static ConnectionHandler instance;
    #endregion
   
    #region Getters and Setters
    public string dataString
    { get; private set; }
    public bool error
    { get; set; }
    public string errorMsg
    { get; private set; }
    public bool loggedIn
    { get; private set; }
    #endregion

    #region URLs
    // Denotes the php scripts used to communicate to the MySQL database
    private const string loginURL = "http://localhost/compSciNeaDB/login.php";
    private const string signupURL = "http://localhost/compSciNeaDB/signup.php";
    private const string saveURL = "http://localhost/compSciNeaDB/saving.php";
    private const string loadURL = "http://localhost/compSciNeaDB/loading.php";
    #endregion
    #endregion

    #region Unity Methods
    void Awake()
    {
        // Ensures that there is only one instance of this in the project
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Ensures this does not get destroyed when loading a new scene e.g. from the menu to the game
    }

    // Start is called before the first frame update
    void Start()
    {
        dataString = "";
        errorMsg = "";
    }
    #endregion

    public void EmptyFieldError()
    {
        error = true;
        errorMsg = "Error: username or password field was empty";
    }

    // Starts a web request for the PHP scripts using the given URL and the form, from the Unity Documentation
    private IEnumerator GetRequest(string url, WWWForm form)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest(); // Communicates with the php script, php scripts can be run on websites which is why they are provided as URLs 

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            // Sees what the result of the attempted connection and sets errorMsg if necessary
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
        form.AddField("password", PasswordEncrypter.SHA256Encryption(p));

        yield return GetRequest(loginURL, form); 
      
        string[] data = dataString.Split("*"); // Splits the data string into an array, the array contains the player data to set

        int success = Convert.ToInt32(data[0]);
        if (success == 1)
        {
            loggedIn = true;

            // Player has just logged in so set player data variables accordingly to what was indicated by the data string
            PlayerData.instance.username = u;
            PlayerData.instance.accCreated = data[1];
            PlayerData.instance.lastSaveDate = data[2];
            StartCoroutine(LoadGame(u));
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
        form.AddField("password", PasswordEncrypter.SHA256Encryption(p));

        yield return GetRequest(signupURL, form); // Waits for the web request to finish before continuing
        
        string[] data = dataString.Split("*");

        int success = Convert.ToInt32(data[0]); // First value is always whether or not the process was successful or not
        if (success == 1)
        {
            loggedIn = true;

            PlayerData.instance.username = u;
            PlayerData.instance.accCreated = data[1];

            // As a new user has been created, this creates a record for the user in the savestbl within the database
            StartCoroutine(SaveGame(PlayerData.instance.currentHealth, PlayerData.instance.currentStamina, PlayerData.instance.posX, PlayerData.instance.posY, PlayerData.instance.points));
        }
        else
        {
            error = true;
            errorMsg = "Error: The user entered already exists";
        }
        dataString = "";
    }

    // Player has logged out therefore reset all player data to default values
    public void LogOut()
    {
        loggedIn = false;
        PlayerData.instance.ResetData();
    }

    public IEnumerator SaveGame(int health, int stamina, float posX, float posY, int points)
    {
        form = new WWWForm();

        // Adding fields is a way to make these variables useable in the php scripts
        form.AddField("username", PlayerData.instance.username);
        form.AddField("points", points);
        form.AddField("currentHealth", health);
        form.AddField("currentStamina", stamina);
        form.AddField("posX", posX.ToString());
        form.AddField("posY", posY.ToString());
        form.AddField("enemiesDefeated", PlayerData.instance.enemiesDefeated);
        form.AddField("colour", PlayerData.instance.colour);

        yield return GetRequest(saveURL, form);

        string[] data = dataString.Split("*");

        int success = Convert.ToInt32(data[0]);
        if (success == 1)
        {
            Debug.Log("Save was a success!");
            PlayerData.instance.lastSaveDate = data[1]; // Gets the piece of data from the returned string
        }
        else
        {
            Debug.Log("Error");
        }
        dataString = "";
    }

    public IEnumerator LoadGame(string u)
    {
        form = new WWWForm(); // new WWWForm object used to send data to the php scripts

        form.AddField("username", u);

        yield return GetRequest(loadURL, form);

        string[] data = dataString.Split("*");

        int success = Convert.ToInt32(data[0]);  
        if (success == 1)
        {
            Debug.Log("Load was a success!");

            // Player has just loaded the game so set the player data values accordingly
            PlayerData.instance.points = Convert.ToInt32(data[1]);
            PlayerData.instance.currentHealth = Convert.ToInt32(data[2]);
            PlayerData.instance.currentStamina = Convert.ToInt32(data[3]);
            PlayerData.instance.posX = float.Parse(data[4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            PlayerData.instance.posY = float.Parse(data[5], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            PlayerData.instance.enemiesDefeated = Convert.ToInt32(data[6]);
            PlayerData.instance.colour = data[7];
        }
        else
        {
            Debug.Log("Error");
        }
        dataString = "";
    }
}
