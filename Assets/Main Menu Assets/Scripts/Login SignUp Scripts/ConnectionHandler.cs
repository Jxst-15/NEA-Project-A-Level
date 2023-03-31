using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Will handle the communcation between Unity and the php scripts provided by URL
public class ConnectionHandler : MonoBehaviour
{
    #region Fields
    public string url
    { get; set; }
    public string dataString
    { get; private set; }

    public WWWForm form;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        dataString = "";
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

    public void ResetDataString()
    {
        dataString = "";
    }

    IEnumerator GetRequest(string url, WWWForm form)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = url.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("Connection Error");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    
                    break;
            }
        }
    }
}
